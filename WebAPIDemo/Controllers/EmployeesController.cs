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
        //By default, the HTTP verb GET is mapped to a method in a controller that has the name Get() or starts with the word Get.
        //If a method is named Get(),by convention it is mapped to the HTTP verb GET. 
        //Even if you rename it to GetEmployees() or GetSomething() it will still be mapped to the HTTP verb GET as long as the 
        //name of the method is prefixed with the word Get. The word Get is case-insensitive. 
        //It can be lowercase, uppercase or a mix of both.

        //If the method is not named Get or if it does not start with the word get then Web API does not know the method name to 
        //which the GET request must be mapped and the request fails with an error message stating The requested resource does not
        //support http method 'GET' with the status code 405 Method Not Allowed. Now if we rename Get() 
        //method to LoadEmployees() and issue a GET request, the request will fail, because ASP.NET Web API does not know it 
        //has to map the GET request to this method.

        //To instruct Web API to map HTTP verb GET to LoadEmployees() method, decorate the method with [HttpGet] attribute.

        //If we add another method as LoadEmployeeById(int id). 

        //When we navigate to the following URI, notice we are getting all the Employees, instead of just the Employee with Id=1. 
        //This is because in this case the GET request is mapped to LoadEmployees() and not LoadEmployeeById(int id). If you want 
        //the GET request to be mapped to LoadEmployeeById(int id) when the id parameter is specified in the URI, decorate 
        //LoadEmployeeById(int id) method also with [HttpGet] attribute. http://localhost:3567/api/Employees/1

        //Attributes that are used to map your custom named methods in the controller class to GET, POST, PUT and DELETE http verbs
        //are [HttpGet], [HttpPost],[HttpPut]and [HttpDelete] respetively.


        [HttpGet]
        public IEnumerable<Employee> LoadEmployees()
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
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
