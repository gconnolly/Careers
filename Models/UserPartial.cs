using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace Careers.Models
{
    public partial class User : IUser
    {
        public const string EMPLOYEE = "employee";
        public const string CANDIDATE = "candidate";

        string IUser<string>.Id
        {
            get { return this.Id.ToString(); }
        }

        public string UserName
        {
            get
            {
                return this.EmailAddress;
            }
            set
            {
                this.EmailAddress = value;
            }
        }

        public bool IsEmployee
        {
            get
            {
                return this.Roles.Any(r => r.Name == EMPLOYEE);
            }
        }

        public bool IsCandidate
        {
            get
            {
                return this.Roles.Any(r => r.Name == CANDIDATE);
            }
        }
    }
}