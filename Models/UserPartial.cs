using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Careers.Models
{
    public partial class User : IUser
    {
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