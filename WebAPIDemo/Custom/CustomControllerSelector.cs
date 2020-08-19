﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebAPIDemo.Custom
{
    // Derive from the DefaultHttpControllerSelector class
    public class CustomControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        public CustomControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            // Get all the available Web API controllers
            var controllers = GetControllerMapping();
            // Get the controller name and parameter values from the request URI
            var routeData = request.GetRouteData();

            // Get the controller name from route data.
            // The name of the controller in our case is "Students"
            var controllerName = routeData.Values["controller"].ToString();

            // Default version number to 1
            string versionNumber = "1";

            // Get the version number from the Custom media type

            // Use regular expression for mataching the pattern of the media
            // type. We have given a name for the matched group that contains
            // the version number. This enables us to retrieve the version number 
            // using the group name("version") instead of ZERO based index
            string regex =
                @"application\/vnd\.godigitalpro\.([a-z]+)\.v(?<version>[0-9]+)\+([a-z]+)"; // application/vnd.godigitalpro.students.v1+json

            // Users can include multiple Accept headers in the request.
            // Check if any of the Accept headers has our custom media type by
            // checking if there is a match with regular expression specified
            var acceptHeader = request.Headers.Accept
                .Where(a => Regex.IsMatch(a.MediaType, regex, RegexOptions.IgnoreCase));
            // If there is atleast one Accept header with our custom media type
            if (acceptHeader.Any())
            {
                // Retrieve the first custom media type
                var match = Regex.Match(acceptHeader.First().MediaType,
                    regex, RegexOptions.IgnoreCase);
                // From the version group, get the version number
                versionNumber = match.Groups["version"].Value;
            }

            if (versionNumber == "1")
            {
                // if version number is 1, then append V1 to the controller name.
                // So at this point the, controller name will become StudentsV1
                controllerName = controllerName + "V1";
            }
            else
            {
                // if version number is 2, then append V2 to the controller name.
                // So at this point the, controller name will become StudentsV2
                controllerName = controllerName + "V2";
            }

            HttpControllerDescriptor controllerDescriptor;
            if (controllers.TryGetValue(controllerName, out controllerDescriptor))
            {
                return controllerDescriptor;
            }

            return null;
        }
    }
}