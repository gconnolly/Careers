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
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private const string PDFMIMETYPE = "application/pdf";
        
        public ResumeController()
            : this(new ApplicationDbContext(), a => new UserManager<User>(new Careers.Models.UserStore(a)))
        {
        }

        public ResumeController(ApplicationDbContext applicationDbContext, Func<ApplicationDbContext, UserManager<User>> createUserManager)
        {
            this.context = applicationDbContext;
            this.userManager = createUserManager(applicationDbContext);
        }

        //
        // GET: /Application/View/5
        public ActionResult View(int id)
        {
            var resume = context.Resumes.SingleOrDefault(r => r.Id == id);
            return new FileContentResult(resume.Document, PDFMIMETYPE);
        }

        public static bool IsValidContentType(string contentType)
        {
            return contentType.ToLower() == PDFMIMETYPE;
        }

        public static byte[] GetDocumentFromFile(HttpPostedFileBase file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                return ms.GetBuffer();
            }
        }
    }
}
