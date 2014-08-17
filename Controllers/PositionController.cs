using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careers.Models
{
    [Authorize]
    [RequireHttps]
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public PositionController()
            : this(new ApplicationDbContext(), a => new UserManager<User>(new Careers.Models.UserStore(a)))
        {
        }

        public PositionController(ApplicationDbContext applicationDbContext, Func<ApplicationDbContext, UserManager<User>> createUserManager)
        {
            this.context = applicationDbContext;
            this.userManager = createUserManager(applicationDbContext);
        }

        //
        // GET: /Position/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var positions = context.Positions;
            var user = userManager.FindById(User.Identity.GetUserId());

            return View(new PositionListingViewModel(positions, user));
        }

        //
        // GET: /Position/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var position = context.Positions.Single(p => p.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (position == null)
            {
                //Unable to find position, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new PositionDetailViewModel(position, user));
        }

        //
        // GET: /Position/Create
        public ActionResult Create()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null || !user.IsEmployee)
            {
                //Unable to find user or position, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }

            return View(new PositionCreateViewModel());
        }

        //
        // POST: /Position/Create
        [HttpPost]
        public ActionResult Create(PositionCreateViewModel positionCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindById(User.Identity.GetUserId());
                if (user == null || !user.IsEmployee)
                {
                    //Unable to find user or position, or unauthorized: return to start
                    return RedirectToAction("Index", "Position", new { });
                }

                var position = new Position
                {
                    Title = positionCreateViewModel.Title,
                    Description = positionCreateViewModel.Description,
                    Status = PositionStatus.Open,
                };

                context.Positions.Add(position);

                context.SaveChanges();

                return RedirectToAction("Details", "Position", new { id = position.Id });
            }

            return View(positionCreateViewModel);
        }

        //
        // GET: /Position/Edit/5
        public ActionResult Edit(int id)
        {
            var position = context.Positions.SingleOrDefault(p => p.Id == id);
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user == null
                || position == null
                || !user.IsEmployee)
            {
                //Unable to find user or position, or unauthorized: return to start
                return RedirectToAction("Index", "Position", new { });
            }


            return View(new PositionEditViewModel(position, user));
        }

        //
        // POST: /Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PositionEditViewModel positionEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var position = context.Positions.Single(p => p.Id == id);
                var user = userManager.FindById(User.Identity.GetUserId());
                if (user == null
                    || position == null
                    || !user.IsEmployee)
                {
                    //Unable to find user or position, or unauthorized: return to start
                    return RedirectToAction("Index", "Position", new { });
                }

                position.Description = positionEditViewModel.Description;
                position.Title = positionEditViewModel.Title;
                position.Status = positionEditViewModel.Status;

                context.SaveChanges();

                return RedirectToAction("Details", "Position", new { id = position.Id });
            }

            return View(positionEditViewModel);
        }
    }
}
