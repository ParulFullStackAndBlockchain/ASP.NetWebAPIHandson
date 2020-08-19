using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
        public class StudentsV2Controller : ApiController
        {
            List<StudentV2> students = new List<StudentV2>()
        {
            new StudentV2() { Id = 1, FirstName = "Tom", LastName = "T"},
            new StudentV2() { Id = 2, FirstName = "Sam", LastName = "S"},
            new StudentV2() { Id = 3, FirstName = "John", LastName = "J"}
        };

        //can be requested via http://localhost:3567/api/students?v=2
        public IEnumerable<StudentV2> Get()
        {
            return students;
        }

        //can be requested via http://localhost:3567/api/students?v=2&id=3
        public StudentV2 Get(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }
    }
}
