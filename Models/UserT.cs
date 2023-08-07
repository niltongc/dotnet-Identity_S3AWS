using Microsoft.AspNetCore.Identity;

namespace exmaple_identity.Models
{
    public class UserT : IdentityUser<int>
    {
        public int Id { get; set; }
        public string NameT { get; set; }
    }
}
