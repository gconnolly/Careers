using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careers.Models
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext context;

        public PositionController()
            : this(new ApplicationDbContext())
        {
        }

        public PositionController(ApplicationDbContext applicationDbContext)
        {
            context = applicationDbContext;
        }

        //
        // GET: /Position/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(context.Positions.Where( p => p.Status == PositionStatus.Open ));
        }

        //
        // GET: /Position/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View(context.Positions.Single(p => p.Id == id));
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
            var position = context.Positions.Single(p => p.Id == id);

            return View(new PositionEditViewModel
            {
                Title = position.Title,
                Description = position.Description,
                Id = position.Id
            });
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
