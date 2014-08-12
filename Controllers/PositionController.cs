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
            : this(new ApplicationDbContext(), new UserManager<User>(new Careers.Models.UserStore(new ApplicationDbContext())))
        {
        }

        public PositionController(ApplicationDbContext applicationDbContext, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.context = applicationDbContext;
        }

        //
        // GET: /Position/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var viewModel = new PositionIndexViewModel
            {
                Positions = context.Positions.Where(p => p.Status == PositionStatus.Open),
                CanAddPosition = User.Identity.GetUserId() != null && userManager.IsInRole(User.Identity.GetUserId(), "employee")
            };

            return View(viewModel);
        }

        //
        // GET: /Position/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var position = context.Positions.Single(p => p.Id == id);
            //TODO: Handle error scenarios

            var viewModel = new PositionDetailViewModel
            {
                Position = position,
                CanModifyPosition = User.Identity.IsAuthenticated && userManager.IsInRole(User.Identity.GetUserId(), "employee"),
                CanApplyToPosition = User.Identity.IsAuthenticated && userManager.IsInRole(User.Identity.GetUserId(), "candidate"),
                CanViewApplications = User.Identity.IsAuthenticated && userManager.IsInRole(User.Identity.GetUserId(), "employee")
            };

            return View(viewModel);
        }

        //
        // GET: /Position/Create
        public ActionResult Create()
        {
            return View(new PositionCreateViewModel());
        }

        //
        // POST: /Position/Create
        [HttpPost]
        public ActionResult Create(PositionCreateViewModel positionCreateViewModel)
        {
            try
            {
                context.Positions.Add(new Position
                {
                    Title = positionCreateViewModel.Title,
                    Description = positionCreateViewModel.Description,
                    Status = PositionStatus.Open
                });

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Position/Edit/5
        public ActionResult Edit(int id)
        {
            if(User.Identity.IsAuthenticated && userManager.IsInRole(User.Identity.GetUserId(), "employee"))
            {
                var position = context.Positions.Single(p => p.Id == id);
                //TODO: handler error scenarios

                return View(new PositionEditViewModel
                {
                    Title = position.Title,
                    Description = position.Description,
                    Id = position.Id
                });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PositionEditViewModel positionEditViewModel)
        {
            try
            {
                var position = context.Positions.Single(p => p.Id == id);

                position.Title = positionEditViewModel.Title;
                position.Description = positionEditViewModel.Description;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Position/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Position/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                context.Positions.Remove(context.Positions.Single(p => p.Id == id));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
