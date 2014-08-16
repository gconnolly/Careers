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
        public ApplicationListingViewModel() { }

        public ApplicationListingViewModel(IQueryable<Application> applications, User user)
        {
            this.Applications = applications
                                    .Where(a => a.UserId == user.Id && a.Status != ApplicationStatus.Removed)
                                    .OrderBy(a => a.AppliedOn);
        }

        public IEnumerable<Application> Applications { get; set; }
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

        #region Properties

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string AppliedOn { get; set; }

        #endregion
    }

    public class ApplicationEditViewModel
    {
        public ApplicationEditViewModel() { }

        public ApplicationEditViewModel(Application application, User user)
        {
            this.ApplicationId = application.Id;
            this.PositionTitle = application.Position.Title;
            this.FullName = application.User.FullName;
            this.EmailAddress = application.User.EmailAddress;
            this.AppliedOn = application.AppliedOn;
            this.PositionStatus = application.Position.Status;
            this.ApplicationStatus = application.Status;
        }

        #region Properties

        [HiddenInput]
        public int ApplicationId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Candidate Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Position")]
        public string PositionTitle { get; set; }

        [Required]
        [Display(Name = "Application Status")]
        public ApplicationStatus ApplicationStatus { get; set; }

        [Required]
        [Display(Name = "Position Status")]
        public PositionStatus PositionStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Applied On")]
        public DateTime AppliedOn { get; set; }


        #endregion
    }

    public class ApplicationDetailViewModel
    {
        public ApplicationDetailViewModel() { }

        public ApplicationDetailViewModel(Application application, User user)
        {
            this.ApplicationId = application.Id;
            this.ResumeId = application.ResumeId;
            this.FullName = application.User.FullName;
            this.EmailAddress = application.User.EmailAddress;
            this.PositionTitle = application.Position.Title;
            this.ApplicationStatus = application.Status;
            this.PositionStatus = application.Position.Status;
            this.AppliedOn = application.AppliedOn;

            if(user != null)
            {
                this.IsDefaultResume = user.DefaultResumeId == application.ResumeId;
                this.CanSetDefaultResume = application.Resume.UserId == user.Id && user.DefaultResumeId != application.ResumeId;
                this.CanViewEditApplicationStatus = user.IsEmployee;
                this.CanRemoveApplication = user.IsCandidate && user.Id == application.UserId;
            }
        }

        #region Properties

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Candidate Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Position")]
        public string PositionTitle { get; set; }

        [Required]
        [Display(Name = "Application Status")]
        public ApplicationStatus ApplicationStatus { get; set; }

        [Required]
        [Display(Name = "Position Status")]
        public PositionStatus PositionStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Applied On")]
        public DateTime AppliedOn { get; set; }

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

        [HiddenInput]
        public int ResumeId { get; set; }

        [HiddenInput]
        public int ApplicationId { get; set; }

        [HiddenInput]
        public bool IsDefaultResume { get; set; }

        [HiddenInput]
        public bool CanSetDefaultResume { get; set; }

        #endregion
        
        #region Privileges

        [HiddenInput]
        public bool CanViewEditApplicationStatus { get; set; }

        [HiddenInput]
        public bool CanRemoveApplication { get; set; }

        #endregion
    }


    public class ApplicationCreateViewModel
    {
        public ApplicationCreateViewModel() { }

        public ApplicationCreateViewModel(Position position, User user)
        {
            this.FullName = user.FullName;
            this.EmailAddress = user.EmailAddress;
            this.PositionTitle = position.Title;
            this.PositionId = position.Id;
            this.UserId = user.Id;

            this.UserDefaultResumeId = user.DefaultResumeId;
            this.UseNewResume = true;
            this.HasDefaultResume = user.DefaultResumeId != null;
        }

        #region Properties

        [HiddenInput]
        public int PositionId { get; set; }

        [HiddenInput]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Candidate Name")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Position")]
        public string PositionTitle { get; set; }

        [HiddenInput]
        public int? UserDefaultResumeId { get; set; }

        [HiddenInput]
        public bool UseNewResume { get; set; }

        [HiddenInput]
        public bool HasDefaultResume { get; set; }

        #endregion

        
    }
}