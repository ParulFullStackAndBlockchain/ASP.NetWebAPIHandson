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

            //Step 2 : Include the following lines of code.this enables CORS globally for the entire application i.e for all 
            //controllers and action methods. Note : Parameters of EnableCorsAttribute. 
            //'origins'-Comma-separated list of origins that are allowed to access the resource.For example "http://www.pragimtech.com, 
            //http://www.mywebsite.com" will only allow ajax calls from these 2 websites. All the others will be blocked. Use "*" to allow all.
            //'headers'-Comma-separated list of headers that are supported by the resource. For example "accept,content-type,origin" 
            //will only allow these 3 headers. Use "*" to allow all. Use null or empty string to allow none.
            //'methods'-Comma-separated list of methods that are supported by the resource. For example "GET,POST" only allows 
            //Get and Post and blocks the rest of the methods. Use "*" to allow all. Use null or empty string to allow none.
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //EnableCors attribute can be applied on a specific controller or controller method. 
            //If applied at a controller level then it is applicable for all methods in the controller.
            //To apply it at the controller level          
            //1.There is no need to create an instance of EnableCorsAttribute in Register() method of WebApiConfig.cs file.
            //Call the EnableCors() method without any parameter values. 
            //config.EnableCors();
            //2.Apply the EnableCorsAttribute on the controller class
            //[EnableCorsAttribute("*", "*", "*")]public class EmployeesController : ApiController{ ... }

            //In the same manner, you can also apply it at a method level if you wish to do so.
            //To disable CORS for a specific action apply[DisableCors] on that specific action

            //When CORS is enabled, the browser sets the origin header of the request to the domain of the site making the request.
            //The server sets Access-Control-Allow-Origin header in the response to either * or the origin that made the request. 
            //* indicates any site is allowed to make the request.
        }
    }
}
