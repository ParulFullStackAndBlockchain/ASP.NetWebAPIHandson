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
        // gender parameter is mapped to the gender parameter sent in the query string    
        public HttpResponseMessage Get(string gender = "All")
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "Value for gender must be Male, Female or All. " + gender + " is invalid.");
                }
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {                   
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    //When an item is not found, instead of returning NULL and status code '200 OK', '404 Not Found' status code is 
                    //returned along with a meaningful message such as "Employee with Id 101 not found".
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with Id " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    //When a new item is created, status code is '201 Item is Created'.
                    //With 201 status code the location i.e URI of the newly created item is also included.
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        employee.ID.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        //If an item is not found, status code '404 Not Found' is returned.
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        //When the deletion is successful, status code '200 OK' is returned.
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //To update employee details whose Id is 1 we issue a Put request to the following URI http://localhost:3567/api/Employees/1
        //We can also include the Id as a query string parameter in place of route data(mentioned above). URI http://localhost:3567/api/Employees?id=1

        //When a PUT request is issued, Web API maps the data in the request to the PUT method parameters in the EmployeesController. 
        //This process is called Parameter Binding.

        //Now let us understand the default convention used by Web API for binding parameters.
        //If the parameter is a simple type like int, bool, double, etc., Web API tries to get the value from the URI
        //(Either from route data or Query String)
        //If the parameter is a complex type like Customer, Employee etc., Web API tries to get the value from the request body
        //So in our case, the id parameter is a simple type, so Web API tries to get the value from the request URI.
        //The employee parameter is a complex type, so Web API gets the value from the request body.

        //We can change this default parameter binding process by using [FromBody]
        //and[FromUri] attributes.Notice in the example below
        //We have decorated id parameter with[FromBody] attribute, this forces Web API to get it from the request body
        //We have decorated employee parameter with [FromUri] attribute, this forces Web API to get employee data from the 
        //URI (i.e Route data or Query String)

        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        //When we try to update an employee whose id does not exist,status code '404 Not Found' is returned
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();

                        //When the update is successful, status code '200 OK' is returned
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
