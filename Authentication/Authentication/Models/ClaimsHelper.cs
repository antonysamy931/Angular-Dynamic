using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Authentication.Models
{
    public static class ClaimsHelper
    {
        public static string GetPrincipalUser(IPrincipal user)
        {
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;
            return (from c in identity.Claims where c.Type == ClaimTypes.NameIdentifier select c.Value).ToList().FirstOrDefault<string>();
        }

        public static List<string> GetClaim(IPrincipal user, string claimNamespace)
        {
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;
            return (from c in identity.Claims where c.Type == claimNamespace select c.Value).ToList();
        }
    }
}