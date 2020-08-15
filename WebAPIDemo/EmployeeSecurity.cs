using EmployeeDataAccess;
using System;
using System.Linq;

namespace WebAPIDemo
{
    public class EmployeeSecurity
    {
        //Checks if the username and password are valid
        public static bool Login(string username, string password)
        {
            using (WebAPIDemoEmployeeDBEntities entities = new WebAPIDemoEmployeeDBEntities())
            {
                return entities.Users.Any(user =>
                       user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.Password == password);
            }
        }
    }
}