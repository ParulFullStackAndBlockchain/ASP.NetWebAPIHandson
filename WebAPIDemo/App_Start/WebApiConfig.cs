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
using System.Web.Http.Dispatcher;
using WebAPIDemo.Custom;

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

            //How to enable Attribute Routing?
            //In ASP.NET Web API 2, Attribute Routing is enabled by default using following line of code.
            config.MapHttpAttributeRoutes();

            //Convention-based routing : When we create a new Web API project using Visual Studio, 
            //a default route is created as shown below.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Replaces the default controller selector with our custom controller selector. 
            config.Services.Replace(typeof(IHttpControllerSelector),new CustomControllerSelector(config));

            //To add the custom media types to the JsonFormatter
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/vnd.godigitalpro.students.v1+json"));
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/vnd.godigitalpro.students.v2+json"));

            //To add the custom media types to the XmlFormatter
            config.Formatters.XmlFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/vnd.godigitalpro.students.v1+xml"));
            config.Formatters.XmlFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/vnd.godigitalpro.students.v2+xml"));

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //config.Filters.Add(new RequireHttpsAttribute());
        }
    }
}

