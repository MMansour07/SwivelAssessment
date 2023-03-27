using Microsoft.AspNet.Identity.EntityFramework;

namespace Swivel.Core.Model
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
