using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Authentication.Controllers
{
    public class LoginController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public string Get(dynamic model)
        {
            Dictionary<string, string> Detail = new Dictionary<string, string>();
            Detail.Add("username", "Antony");
            Detail.Add("loggedin", DateTime.Now.ToString());
            Detail.Add("expiredin", DateTime.Now.AddMinutes(5).ToString());
            
            string json = JsonConvert.SerializeObject(Detail, Formatting.None);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        [AllowAnonymous]
        [HttpGet]
        public string Logout()
        {            
            if(HttpContext.Current.Session != null)
                HttpContext.Current.Session.Abandon();
            return "success";
        }
    }
}
