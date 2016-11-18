using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TypeScrip_ex.Controllers
{
    public class HomeController : Controller
    {
        public void Index()
        {
            Response.Redirect("/Application/Index.html#");
        }
    }
}
