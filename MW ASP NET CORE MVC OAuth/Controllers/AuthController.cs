using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using MW_ASP_NET_CORE_MVC_OAuth.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace MW_ASP_NET_CORE_MVC_OAuth.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("oauth-login")]
        public IActionResult OAuthLogin(OAuthType type)
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("OAuthResponse") };
            switch (type)
            {
                case OAuthType.Google:
                    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
                case OAuthType.MicrosoftAccount:
                    return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
                case OAuthType.Facebook:
                    return Challenge(properties, FacebookDefaults.AuthenticationScheme);
                case OAuthType.GitHub:
                    return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
                default:
                    return Redirect(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.AuthenticationType == "AuthApp")
            {
                return RedirectToAction("Index", "Profile");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("oauth-response")]
        public async Task<IActionResult> OAuthResponse()
        {
            var claims = HttpContext.User.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return await Auth(claims.Where(c => c.Type.EndsWith("/name")).FirstOrDefault()?.Value ?? String.Empty,
                claims.Where(c => c.Type.EndsWith("/emailaddress")).FirstOrDefault()?.Value ?? String.Empty);
        }

        private async Task<IActionResult> Auth(string name, string email)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim("Email", email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                "AuthApp",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Profile");
        }
    }
}
