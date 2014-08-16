using Careers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careers.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public ApplicationController()
            : this(new ApplicationDbContext(), a => new UserManager<User>(new Careers.Models.UserStore(a)))
        {
        }

        public ApplicationController(ApplicationDbContext applicationDbContext, Func<ApplicationDbContext, UserManager<User>> createUserManager)
        {
            this.context = applicationDbContext;
            this.userManager = createUserManager(applicationDbContext);
        }

        //
        // GET: /Application/Index
        public ActionResult Index()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                //Unable to find user: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new Careers.Models.ApplicationListingViewModel(context.Applications, user));
        }

        //
        // GET: /Application/Details/5
        public ActionResult Details(int id)
        {
            var application = context.Applications.SingleOrDefault(a => a.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null
                || application == null
                || (application.UserId != user.Id && !user.IsEmployee))
            {
                //Unable to find user or application, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new Careers.Models.ApplicationDetailViewModel(application, user));
        }

        //
        // GET: /Application/Create/5
        public ActionResult Create(int id)
        {
            var position = context.Positions.SingleOrDefault(p => p.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null
                || position == null
                || !user.IsCandidate)
            {
                //Unable to find user or application, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new Careers.Models.ApplicationCreateViewModel(position, user));
        }

        //
        // POST: /Application/Create
        [HttpPost]
        public ActionResult Create(ApplicationCreateViewModel applicationCreateViewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindById(User.Identity.GetUserId());
                if (user == null || !user.IsCandidate)
                {
                    //Unable to find user or application, or unauthorized: return to start
                    return RedirectToAction("Index", "Position", new { });
                }

                //Only accept PDF file types
                if (applicationCreateViewModel.UseNewResume && (file == null
                    || !ResumeController.IsValidContentType(file.ContentType)
                    || file.ContentLength == 0))
                {
                    return View(applicationCreateViewModel);
                }

                var application = new Application
                {
                    AppliedOn = DateTime.Now.Date,
                    PositionId = applicationCreateViewModel.PositionId,
                    UserId = applicationCreateViewModel.UserId
                };

                application.Resume = applicationCreateViewModel.UseNewResume
                    ? new Resume
                    {
                        Document = ResumeController.GetDocumentFromFile(file),
                        UserId = applicationCreateViewModel.UserId
                    }
                    : user.Resume;

                context.Applications.Add(application);
                context.SaveChanges();

                return RedirectToAction("Details", new { id = application.Id });
            }

            return View(applicationCreateViewModel);
        }

        //
        // GET: /Application/Edit/5
        public ActionResult Edit(int id)
        {
            var application = context.Applications.SingleOrDefault(a => a.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null
                || application == null
                || (application.UserId != user.Id && !user.IsEmployee))
            {
                //Unable to find user or application, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new Careers.Models.ApplicationEditViewModel(application, user));
        }

        //
        // POST: /Application/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ApplicationEditViewModel applicationEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var application = context.Applications.SingleOrDefault(a => a.Id == id);
                var user = userManager.FindById(User.Identity.GetUserId());

                if (user == null
                    || application == null
                    || (application.UserId != user.Id && !user.IsEmployee))
                {
                    //Unable to find user or application, or unauthorized: return to start
                    return RedirectToAction("Index", "Position", new { });
                }

                application.Status = applicationEditViewModel.ApplicationStatus;
                context.SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }

            return View(applicationEditViewModel);
        }

        //
        // POST: /Application/Remove/5
        public ActionResult Remove(int id)
        {
            var application = context.Applications.SingleOrDefault(a => a.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());

            if (user == null
                || application == null
                || application.UserId != user.Id)
            {
                //Unable to find user or application, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            application.Status = ApplicationStatus.Removed;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
