using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        //Depending on the Accept header value in the request, the server sends the response. 
        //This is called Content Negotiation.If you don't specify the Accept header, by default the Web API returns JSON data.

        //When the response is being sent to the client in the requested format, notice that the Content-Type header of the 
        //response is set to the appropriate value. For example, if the client has requested application/xml as 'Accept: application/xml', 
        //the server send the data in XML format and also sets the Content-Type=application/xml. 

        //ASP.NET Web API is greatly extensible. This means we can also plugin our own formatters, for custom formatting the data.
        //Multiple values can also be specified for the Accept header.In this case, the server picks the first formatter which is a
        //JSON formatter and formats the data in JSON. Accept: application/xml, application/json
        //You can also specify quality factor.In the example below, xml has higher quality factor than json, so the server uses XML
        //formatter and formats the data in XML.application/xml;q= 0.8, application/json;q= 0.5

        //What does the Web API do when we request for data in a specific format?
        //The Web API controller generates the data that we want to send to the client.For example, if you have asked for list of 
        //employees.The controller generates the list of employees, and hands the data to the Web API pipeline which then looks at 
        //the Accept header and depending on the format that the client has requested, Web API will choose the appropriate formatter.
        //For example, if the client has requested for XML data via accept header 'Accept: application/xml', Web API uses XML formatter . 
        //If the client has requested for JSON data via accept header 'Accept: application/json', Web API uses JSON formatter.
        //These formatters are called Media type formatters.

        //The formatters are used by the server for both request and response messages. When the client sends a request to the 
        //server, we set the Content-Type header to the appropriate value to let the server know the format of the data that we are
        //sending. For example, if the client is sending JSON data, the Content-Type header is set to application/json. The server
        //knows it is dealing with JSON data, so it uses JSON formatter to convert JSON data to .NET Type. Similarly when a 
        //response is being sent from the server to the client, depending on the Accept header value, the appropriate formatter is 
        //used to convert .NET type to JSON, XML etc.


        public IEnumerable<Employee> Get()
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public Employee Get(int id)
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
            }
        }
    }
}
