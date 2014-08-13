using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
                return this.FullName;
            }
            set
            {
                this.FullName = value;
            }
        }
    }
}