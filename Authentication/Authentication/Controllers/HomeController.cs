using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = User as ClaimsPrincipal;
            var identity = user.Identity as ClaimsIdentity;
            var name = (from o in identity.Claims where o.Type == ClaimTypes.Name select o.Value).FirstOrDefault();
            var role = (from o in identity.Claims where o.Type == ClaimTypes.Role select o.Value).FirstOrDefault();
            var nameidentifier = (from o in identity.Claims where o.Type == ClaimTypes.NameIdentifier select o.Value).FirstOrDefault();
            return View();
        }

        public ActionResult Account()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Antony"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "0001"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "UserSTS");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            //var token = new SessionSecurityToken(principal);
            //var sam = FederatedAuthentication.SessionAuthenticationModule;
            //sam.WriteSessionTokenToCookie(token);
            Thread.CurrentPrincipal = principal;
            return RedirectToAction("Index");
        }
    }
}
