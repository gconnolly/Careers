using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Careers.Models
{
    public class PositionEditViewModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        public PositionStatus Status { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int Id { get; set; }
    }

    public class PositionCreateViewModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class PositionIndexViewModel
    {
        public IEnumerable<Position> Positions { get; set; }

        public bool CanAddPosition { get; set; }
    }

    public class PositionDetailViewModel
    {
        public Position Position { get; set; }

        public bool CanModifyPosition { get; set; }

        public bool CanViewApplications { get; set; }

        public bool CanApplyToPosition { get; set; }
    }
}