using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MW_ASP_NET_CORE_MVC_OAuth.Models.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace MW_ASP_NET_CORE_MVC_OAuth.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var claims = HttpContext.User.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            UserViewModel user = new UserViewModel() { Name = claims.ElementAtOrDefault(0)?.Value,
                Email = claims.ElementAtOrDefault(1)?.Value,
                Role = claims.ElementAtOrDefault(2)?.Value };
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

    }
}
