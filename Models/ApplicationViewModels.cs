using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careers.Models
{
    public class ApplicationListingViewModel
    {
        private readonly User user;

        public ApplicationListingViewModel() { }

        public ApplicationListingViewModel(IQueryable<Application> applications, User user)
        {
            this.user = user;
            this.Applications = applications.Where(a => a.UserId == user.Id && a.Status != ApplicationStatus.Removed).OrderBy(a => a.AppliedOn);
        }

        public IEnumerable<Application> Applications { get; private set; }
    }

    public class ApplicationListItemViewModel
    {
        public ApplicationListItemViewModel() { }

        public ApplicationListItemViewModel(Application application)
        {
            this.Id = application.Id;
            this.UserName = application.User.FullName;
            this.Status = application.Status.ToString();
            this.AppliedOn = application.AppliedOn.ToShortDateString();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string AppliedOn { get; set; }
    }

    public class ApplicationViewModel
    {
        private readonly User user;

        public ApplicationViewModel() { }

        public ApplicationViewModel(Application application, User user)
        {
            this.user = user;
            this.ApplicationId = application.Id;
            this.PositionId = application.Position.Id;
            this.ResumeId = application.ResumeId;
            this.UserId = application.User.Id;
            this.UserName = application.User.FullName;
            this.EmailAddress = application.User.EmailAddress;
            this.PositionTitle = application.Position.Title;
            this.ApplicationStatus = application.Status;
            this.PositionStatus = application.Position.Status;
            this.AppliedOn = application.AppliedOn;
        }

        public int UserId { get; set; }

        public int PositionId { get; set; }

        public int ResumeId { get; private set; }

        public int ApplicationId { get; private set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Candidate Name")]
        public string UserName { get; private set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; private set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Position")]
        public string PositionTitle { get; private set; }

        [Required]
        [Display(Name = "Application Status")]
        public ApplicationStatus ApplicationStatus { get; set; }

        [Required]
        [Display(Name = "Position Status")]
        public PositionStatus PositionStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Applied On")]
        public DateTime AppliedOn { get; private set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Applied On")]
        public string AppliedOnDisplay
        {
            get
            {
                return AppliedOn.ToShortDateString();
            }
        }

        public bool CanViewEditApplicationStatus
        {
            get
            {
                return user != null
                    && user.Roles.Any(r => r.Name == User.EMPLOYEE);
            }
        }

        public bool CanRemoveApplication
        {
            get
            {
                return user != null
                    && user.Roles.Any(r => r.Name == User.CANDIDATE)
                    && user.Id == UserId;
            }
        }
    }
}