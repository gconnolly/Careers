using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Careers.Models
{
    public class ApplicationCreateViewModel
    {
        public string UserId { get; set; }

        public int PositionId { get; set; }

        public string PositionTitle { get; set; }
    }

    public class ApplicationDetailsViewModel
    {
        public Application Application { get; set; }
    }
}