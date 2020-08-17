using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{   
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        static List<Student> students = new List<Student>()
        {
            new Student() { Id = 1, Name = "Tom" },
            new Student() { Id = 2, Name = "Sam" },
            new Student() { Id = 3, Name = "John" }
        };

        [Route("")]
        public IEnumerable<Student> Get()
        {
            return students;
        }
        
        [Route("~/api/teachers")]
        public IEnumerable<Teacher> GetTeachers()
        {
            List<Teacher> teachers = new List<Teacher>()
            {
                new Teacher() { Id = 1, Name = "Rob" },
                new Teacher() { Id = 2, Name = "Mike" },
                new Teacher() { Id = 3, Name = "Mary" }
            };

            return teachers;
        }

        //[Route("{id}")]
        //[Route("{id:int}")] => "{parameter:constraint}"
        //[Route("{id:int:min(1):max(3)}")] => if you want the id value in the URI to be between 1 and 3 inclusive
        [Route("{id:int:range(1,3)}")] // Using range attribute constraint in place of min and max.
        public Student Get(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        // [Route("{name}")] => At this point build the solution, and if you navigate to either of the following URI's you get an error
        //stating "Multiple actions were found that match the request" /api/students/1 or /api/students/Sam
        //This is because the framework does not know which version of the Get() method to use

        //This can be very easily achieved using Route Constraints. To specify route constraint, the syntax is 
        //"{parameter:constraint}". like 

        [Route("{name:alpha}")]
        public Student Get(string name)
        {
            return students.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        }

        [Route("{id}/courses")]
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
