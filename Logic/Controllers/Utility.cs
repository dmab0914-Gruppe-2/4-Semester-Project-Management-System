using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Controllers
{
    public class Utility
    {
        public object Sanitizer(object input)
        {
            
            if (input is Project)
            {
                Project project = new Project();
                input = project;
                project.Description = Sanitizer(project.Description);
                project.Title = Sanitizer(project.Title);
                project.LeaderUser.Email = Sanitizer(project.LeaderUser.Email);
                project.LeaderUser.Username = Sanitizer(project.LeaderUser.Username);
                return project;


            }
            if (input is Models.Task)
            {
                Models.Task task = new Models.Task();
                

            }



            //throw new NotImplementedException("Didn't finish this function yet.");
            return null;
        }

        public string Sanitizer(string input)
        {
            if (input.Contains("'"))
            {
                string output = input.Replace("'", "''");
                return output;
            }
            return input;
        }
    }
}
