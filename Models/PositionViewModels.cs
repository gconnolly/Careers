using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Careers.Models
{
    public class PositionListingViewModel
    {
        private readonly User user;

        public PositionListingViewModel() { }

        public PositionListingViewModel(IQueryable<Position> positions, User user)
        {
            this.user = user;
            this.Positions = positions.Where(p => p.Status == PositionStatus.Open).OrderBy(p => p.Id);
        }

        public IEnumerable<Position> Positions { get; private set; }

        public bool CanAddPosition
        {
            get
            {
                return user != null
                    && user.Roles.Any(r => r.Name == User.EMPLOYEE);
            }
        }
    }

    public class PositionViewModel
    {
        private readonly User user;

        public PositionViewModel() { }

        public PositionViewModel(Position position, User user)
        {
            this.user = user;
            this.PositionId = position.Id;
            this.Title = position.Title;
            this.Description = position.Description;
            this.Status = position.Status;
            this.Applications = position.Applications.Where(a => a.Status != ApplicationStatus.Removed).OrderBy(a => a.AppliedOn);

            var application = user.Applications.SingleOrDefault(a => a.PositionId == position.Id && a.Status != ApplicationStatus.Removed);
            if(application != null)
            {
                this.UserApplicationId = application.Id;
                this.UserAppliedOn = application.AppliedOn;
            }
            
        }

        public int UserApplicationId { get; private set; }

        public DateTime UserAppliedOn { get; private set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int PositionId { get; set; }

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

        public IEnumerable<Application> Applications { get; private set; }

        public bool CanModifyPosition
        {
            get
            {
                return user != null && user.Roles.Any(r => r.Name == User.EMPLOYEE);
            }
        }

        public bool CanViewApplications
        {
            get
            {
                return user != null && user.Roles.Any(r => r.Name == User.EMPLOYEE);
            }
        }

        public bool CanApplyToPosition
        {
            get
            {
                return user == null
                    || (user.Roles.Any(r => r.Name == User.CANDIDATE)
                    && !user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed));
            }
        }

        public bool CanRemoveApplication
        {
            get
            {
                return user == null
                    || (user.Roles.Any(r => r.Name == User.CANDIDATE)
                    && !user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed));
            }
        }

        public bool HasAppliedToPosition
        {
            get
            {
                return user != null
                    && user.Roles.Any(r => r.Name == User.CANDIDATE)
                    && user.Applications.Any(a => a.PositionId == this.PositionId && a.Status != ApplicationStatus.Removed);
            }
        }
    }
}