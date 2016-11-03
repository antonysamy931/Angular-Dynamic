﻿using Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Authentication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var authenticationHandler = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(config),
                    new DelegatingHandler[] { new AuthenticateHandler() });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints : null,
                handler: authenticationHandler
            );
        }
    }
}
