using System.ComponentModel;

namespace MW_ASP_NET_CORE_MVC_OAuth.Models.ViewModel
{
    public class UserViewModel
    {
        [DisplayName("User Name:")]
        public string Name { get; set; }
        [DisplayName("E-mail:")]
        public string Email { get; set; }
        [DisplayName("Role:")]
        public string Role { get; set; }
    }
}
