using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{       
    public class StudentsController : ApiController
    {
        static List<Student> students = new List<Student>()
        {
            new Student() { Id = 1, Name = "Tom" },
            new Student() { Id = 2, Name = "Sam" },
            new Student() { Id = 3, Name = "John" }
        };

        //Retrurn Type : HttpResponseMessage
        //public HttpResponseMessage Get()
        //{
        //    return Request.CreateResponse(students);
        //}

        //Retrurn Type : IHttpActionResult
        public IHttpActionResult Get()
        {
            //To return status code 200, we use Ok() helper method
            return Ok(students);
        }


        //Retrurn Type : HttpResponseMessage
        //public HttpResponseMessage Get(int id)
        //{
        //    var student = students.FirstOrDefault(s => s.Id == id);
        //    if (student == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //            "Student not found");
        //    }

        //    return Request.CreateResponse(student);
        //}

        //Retrurn Type : IHttpActionResult
        public IHttpActionResult Get(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                //to return status code 404, we used NotFound() method
                //return NotFound();
                return Content(HttpStatusCode.NotFound, "Student not found");
            }

            return Ok(student);
        }

        //In addition to Ok() and NotFound() helper methods, we have the following methods that we can use depending on what we want 
        //to return from our controller action method.All these methods return a type, that implements IHttpActionResult interface.
        //BadRequest(), Conflict(), Created(), InternalServerError(), Redirect(), Unauthorized()
    }
}
//In Web API 1, we have HttpResponseMessage type that a controller action method returns. A new type called "IHttpActionResult" 
//is introduced in Web API 2 that can be returned from a controller action method. Instead of returning HttpResponseMessage from a 
//controller action, we can now return IHttpActionResult. There are 2 main advantages of using the IHttpActionResult interface.
//1. The code is cleaner and easier to read
//2 .Unit testing controller action methods is much simpler.
