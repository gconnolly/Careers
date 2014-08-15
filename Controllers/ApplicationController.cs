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

            return View(new Careers.Models.ApplicationViewModel(application, user));
        }

        //
        // GET: /Application/Create/5
        public ActionResult Create(int id)
        {
            var position = context.Positions.SingleOrDefault(p => p.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null
                || position == null
                || !user.IsEmployee)
            {
                //Unable to find user or application, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            var application = new Application
            {
                Position = position,
                User = user,
                Status = ApplicationStatus.New,
                AppliedOn = DateTime.Now.Date
            };

            return View(new Careers.Models.ApplicationViewModel(application, user));
        }

        //
        // POST: /Application/Create
        [HttpPost]
        public ActionResult Create(ApplicationViewModel applicationViewModel, HttpPostedFileBase file)
        {
            //Only accept PDF file types
            if (!ResumeController.IsValidContentType(file.ContentType)) { return View(); }

            try
            {
                var application = new Application
                {
                    AppliedOn = DateTime.Now.Date,
                    PositionId = applicationViewModel.PositionId,
                    UserId = applicationViewModel.UserId,
                    Resume = new Resume
                    {
                        Document = ResumeController.GetDocumentFromFile(file),
                        UserId = applicationViewModel.UserId
                    }
                };

                context.Applications.Add(application);
                context.SaveChanges();

                return RedirectToAction("Details", new { id = application.Id });
            }
            catch
            {
                return View();
            }
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

            return View(new Careers.Models.ApplicationViewModel(application, user));
        }

        //
        // POST: /Application/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ApplicationViewModel applicationViewModel)
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

            try
            {
                application.Status = applicationViewModel.ApplicationStatus;

                context.SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }
            catch
            {
                return View();
            }
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

            try
            {
                application.Status = ApplicationStatus.Removed;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
