//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Careers.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Applications = new HashSet<Application>();
            this.Resumes = new HashSet<Resume>();
            this.UserLogins = new HashSet<UserLogin>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<int> DefaultResumeId { get; set; }
    
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; }
        public virtual Resume Resume { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
