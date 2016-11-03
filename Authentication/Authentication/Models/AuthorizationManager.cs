using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Authentication.Models
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            string resource = context.Resource.First().Value;
            string action = context.Action.First().Value;

            switch (action)
            {
                case "Values":
                    return CheckAccessValues(context.Principal, action);
                case "Admin":
                    return true;
                default:
                    return false;
            }
        }

        private bool CheckAccessValues(ClaimsPrincipal claimsPrincipal, string action)
        {
            bool hasAccess = false;
            List<string> roles = ClaimsHelper.GetClaim(claimsPrincipal, ClaimTypes.Role);
            if (roles != null && roles.Count > 0 && roles.Contains<string>("Admin"))
                hasAccess = true;
            return hasAccess;
        }
    }
}