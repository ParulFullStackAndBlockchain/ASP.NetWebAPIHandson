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

        public IEnumerable<Student> Get()
        {
            return students;
        }

        //Can we use both Attribute Routing and Convention-based routing in a single Web API project ?
        //Yes, both the routing mechanisms can be combined in a single Web API project.The controller action methods that have 
        //the[Route] attribute uses Attribute Routing, and the others without[Route] attribute uses Convention-based routing.
        public Student Get(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        //What is Attribute Routing?
        //Using the[Route] attribute to define routes is called Attribute Routing

        //What are the advantages of using Attribute Routing?
        //Attribute routing gives us more control over the URIs than convention-based routing.Creating URI patterns like hierarchies of
        //resources (For example, students have courses, Departments have employees) is very difficult with convention-based routing.
        //With attribute routing all you have to do is use the [Route] attribute as shown below.
        [Route("api/students/{id}/courses")]
        public IEnumerable<string> GetStudentCourses(int id)
        {
            if (id == 1)
                return new List<string>() { "C#", "ASP.NET", "SQL Server" };
            else if (id == 2)
                return new List<string>() { "ASP.NET Web API", "C#", "SQL Server" };
            else
                return new List<string>() { "Bootstrap", "jQuery", "AngularJs" };
        }
    }
}
