using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Authentication.Models
{
    public class AuthenticateHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!SkipAuthentication(request))
            {
                var authorizationHeader = request.Headers.Authorization;
                if (authorizationHeader != null)
                {
                    var authValue = authorizationHeader.Parameter;
                    if (!string.IsNullOrEmpty(authValue))
                    {
                        byte[] bytes = Convert.FromBase64String(authValue);
                        string json = Encoding.UTF8.GetString(bytes);
                        var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        if (user != null && user.Count > 0)
                        {
                            var userName = user.Where(x => x.Key == "username").Select(x => x.Value).FirstOrDefault();
                            ClaimsPrincipal principal = GetPrincipal("");                            
                            HttpContext.Current.User = principal;
                            Thread.CurrentPrincipal = principal;
                        }
                    }
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        protected bool SkipAuthentication(HttpRequestMessage request)
        {
            string[] skip = new string[] {
                "login"                
            };
            return request.RequestUri.LocalPath.Split('/').ToArray<string>().Any(x => skip.Any(y => y.ToLower() == x.ToLower()));
        }

        private ClaimsPrincipal GetPrincipal(string userid)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Antony"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "0001"));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "UserSTS");
            return new ClaimsPrincipal(identity);
        }
    }
}