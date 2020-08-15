using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using WebApiContrib.Formatting.Jsonp;
using System.Web.Http.Cors;

namespace WebAPIDemo
{
    public static class WebApiConfig
    {
        
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //This adds RequireHttpsAttribute as a filter to the filters collection. So for every request the code in this filter
            //is executed. If the request is issued using HTTP, it will be automatically redirected to HTTPS.
            config.Filters.Add(new RequireHttpsAttribute());
        }
    }
}

//Note : If you don't want to enable HTTPS for the entire application then don't add RequireHttpsAttribute to the filters collection
//on the config object in the register method. Simply decorate the controller class or the action method with RequireHttpsAttribute 
//for which you want HTTPS to be enabled. For the rest of the controllers and action methods HTTPS will not be enabled.

