using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Careers.Models
{
    public class PositionListingViewModel
    {
        public PositionListingViewModel() { }

        public PositionListingViewModel(IQueryable<Position> positions, User user)
        {
            this.Positions = positions.OrderBy(p => p.Id);


            if (user != null)
            {
                this.CanAddPosition = user.IsEmployee;
                this.CanViewClosedPositions = user.IsEmployee;
            }

            if (!this.CanViewClosedPositions)
            {
                this.Positions = this.Positions.Where(p => p.Status == PositionStatus.Open);
            }
        }

        public IEnumerable<Position> Positions { get; set; }

        #region Privileges

        public bool CanAddPosition { get; set; }

        public bool CanViewClosedPositions { get; set; }

        #endregion
    }

    public class PositionCreateViewModel
    {
        public PositionCreateViewModel() { }

        #region Properties

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        #endregion
    }

    public class PositionEditViewModel
    {
        public PositionEditViewModel() { }

        public PositionEditViewModel(Position position, User user)
        {
            this.PositionId = position.Id;
            this.OriginalTitle = position.Title;
            this.Title = position.Title;
            this.Description = position.Description;
            this.Status = position.Status;
        }

        #region Properties

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int PositionId { get; set; }

        [HiddenInput]
        public string OriginalTitle { get; set; }

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

        #endregion
    }

    public class PositionDetailViewModel
    {
        public PositionDetailViewModel() { }

        public PositionDetailViewModel(Position position, User user)
        {
            this.PositionId = position.Id;
            this.Title = position.Title;
            this.Description = position.Description;
            this.Status = position.Status;

            this.Applications = position.Applications
                .Where(a => a.Status != ApplicationStatus.Removed)
                .OrderBy(a => a.AppliedOn)
                .Select( a => new ApplicationListItemViewModel(a));

            if (user != null)
            {
                var application = user.Applications.SingleOrDefault(a => a.PositionId == position.Id && a.Status != ApplicationStatus.Removed);

                if (application != null)
                {
                    this.UserApplicationId = application.Id;
                    this.UserAppliedOn = application.AppliedOn;
                }

                this.CanModifyPosition = user.IsEmployee;
                this.CanViewApplications = user.IsEmployee;
                this.CanApplyToPosition = user.IsCandidate && !user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed);
                this.CanRemoveApplication = user.IsCandidate && user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed);
                this.HasAppliedToPosition = user.IsCandidate && user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed);
            }
            
        }

        #region Properties

        [Display(Name = "ID")]
        public int PositionId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public PositionStatus Status { get; set; }

        public int UserApplicationId { get; set; }

        public DateTime UserAppliedOn { get; set; }

        public IEnumerable<ApplicationListItemViewModel> Applications { get; set; }

        #endregion

        #region Privileges

        public bool CanModifyPosition { get; set; }

        public bool CanViewApplications { get; set; }

        public bool CanApplyToPosition { get; set; }

        public bool CanRemoveApplication { get; set; }

        public bool HasAppliedToPosition { get; set; }

        #endregion
    }
}