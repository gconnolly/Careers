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
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext context;

        public ApplicationController()
            : this(new ApplicationDbContext())
        {
        }

        public ApplicationController(ApplicationDbContext applicationDbContext)
        {
            context = applicationDbContext;
        }

        //
        // GET: /Application/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Application/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Application/Create/5
        public ActionResult Create(int positionId)
        {
            var position = context.Positions.Single(p => p.Id == positionId);

            return View(new Careers.Models.ApplicationCreateViewModel
            {
                PositionId = position.Id,
                PositionTitle = position.Title,
                UserId = User.Identity.GetUserId()
            });
        }

        //
        // POST: /Application/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, int userId, int positionId)
        {
            Application application;
            Resume resume;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    resume = new Resume
                    {
                        Document = array,
                        UserId = userId
                    };
                    application = new Application
                    {
                        AppliedOn = DateTime.Now.Date,
                        PositionId = positionId,
                        UserId = userId,
                        Resume = resume
                    };

                    context.Resumes.Add(resume);
                    context.Applications.Add(application);
                    context.SaveChanges();
                }

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
            return View();
        }

        //
        // POST: /Application/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Application/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Application/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
