using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Controllers
{
    public class Utility
    {
        /// <summary>
        /// Takes an object as input which can either be of the type Project and Task. 
        /// Sanitizes everything within given object and returns a safe to execute to sql version
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Project or Task which is safe to execute variables against sql server</returns>
        public object Sanitizer(object input)
        {
            //foreach (FieldInfo property in input.GetType().GetFields())
            //{
            //    if (property.GetType() is String)
            //    {
                    
            //    }
            //}

            if (input is Project)
            {
                Project project = new Project();
                project = (Project)input;
                if (project.Description != null)
                    project.Description = Sanitizer(project.Description);
                if (project.Title != null)
                    project.Title = Sanitizer(project.Title);
                if (project.LeaderUser != null)
                {
                    if (project.LeaderUser.Email != null)
                        project.LeaderUser.Email = Sanitizer(project.LeaderUser.Email);
                    if (project.LeaderUser.Username != null)
                        project.LeaderUser.Username = Sanitizer(project.LeaderUser.Username);
                }
                return project;


            }
            if (input is Models.Task)
            {
                Models.Task task = new Models.Task();
                task = (Models.Task)input;
                if (task.Description != null)
                    task.Description = Sanitizer(task.Description);
                if (task.Title != null)
                    task.Title = Sanitizer(task.Title);
                if (task.AssignedUser != null)
                {
                    if (task.AssignedUser.Email != null)
                        task.AssignedUser.Email = Sanitizer(task.AssignedUser.Email);
                    if (task.AssignedUser.Username != null)
                        task.AssignedUser.Username = Sanitizer(task.AssignedUser.Username);
                }
                return task;

            }
            return null;


            //throw new NotImplementedException("Didn't finish this function yet.");
            
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
