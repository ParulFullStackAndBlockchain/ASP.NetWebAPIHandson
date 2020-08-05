using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace WebAPIDemo
{
    public static class WebApiConfig
    {
        //To return JSON instead of XML from ASP.NET Web API Service when a request is made from the browser.
        //But When the request is issued from a tool like fiddler the Accept header value should be respected. 
        //Approach-2
        public class CustomJsonFormatter : JsonMediaTypeFormatter
        {
            public CustomJsonFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }

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

            //To return only JSON from ASP.NET Web API Service irrespective of the Accept header value
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //To return only XML from ASP.NET Web API Service irrespective of the Accept header value
            //config.Formatters.Remove(config.Formatters.JsonFormatter);

            //To return JSON instead of XML from ASP.NET Web API Service when a request is made from the browser.
            //Approach-1
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //To return JSON instead of XML from ASP.NET Web API Service when a request is made from the browser.
            //But When the request is issued from a tool like fiddler the Accept header value should be respected. 
            //Approach-2
            config.Formatters.Add(new CustomJsonFormatter());//Registers the formatter

        }
    }
}
