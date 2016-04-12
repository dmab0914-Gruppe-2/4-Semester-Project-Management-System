using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;
using Logic.Models;
using Task = Logic.Models.Task;

namespace DataAccessConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Here we go!");
            //TestProject();
            //TestTask();
            //TestGetTastsByProject();
            TestProjectIdWhenAddingToDB();
            Console.ReadLine();
        }

        private static void TestGetTastsByProject()
        {
            DbTask dbTask = new DbTask();
            Console.WriteLine("1 Task id = " + dbTask.GetTasksByProject(1)[0].Id);
        }

        private static void TestTask()
        {
            DbTask dbTask = new DbTask();
            Console.WriteLine("##Testing retrieval of getting a Task.");
            PrintTaskInformation(dbTask.GetTask(1));
            Console.WriteLine("##DONE");
        }

        private static void TestProjectIdWhenAddingToDB()
        {
            DbProject dbProject = new DbProject();
            Project project = new Project
            {
                Title = "Quick Test",
                Description = "Fancy description",
                Done = true,
                CreatedDate = DateTime.UtcNow,
                LastChange = DateTime.UtcNow
            };
            dbProject.AddProject(project);
            
            Console.WriteLine("Project id is: " + project.Id);
        }

        private static void TestProject()
        {
            DbProject dbProject = new DbProject();
            Console.WriteLine("##Testing retrieval of 1'st project: Project 1##");
            PrintProjectInformation(dbProject.GetProject(1));
            Console.WriteLine("##Testing retrieval of all projects..##");
            List<Project> projects = dbProject.GetAllProjects();
            foreach (var project in projects)
            {
                PrintProjectInformation(project);
            }
            //sConsole.WriteLine(DateTime.UtcNow);
            Console.WriteLine("##Adding project to DB##");
            Project addingproject = new Project();
            addingproject.Title = "The project addition";
            addingproject.CreatedDate = DateTime.UtcNow;
            addingproject.Description = "This project got added from the application";
            addingproject.Done = false;
            addingproject.LastChange = DateTime.UtcNow;
            dbProject.AddProject(addingproject);
            Console.WriteLine("##Project added to db##\nLets test retrieving it.");
            Console.WriteLine("Retrieving added proejct..");
            addingproject = dbProject.GetProject(addingproject.Title).FirstOrDefault();
            if (addingproject.Id != null)
            {
                Console.WriteLine("Project id is: " + addingproject.Id.Value);
                PrintProjectInformation(dbProject.GetProject(addingproject.Id.Value));
                Console.WriteLine("##Removing previously added proejct.. Sucess?:" + dbProject.RemoveProject(addingproject.Id.Value));
            }
            else
            {
                Console.WriteLine("Project id is null..");
            }
            Console.WriteLine("##DONE##");
            
        }

        private static void PrintProjectInformation(Project project)
        {
            if (project == null)
            {
                Console.WriteLine("That project didn't exist or something else happened..");
            }
            else
            {
                Console.WriteLine("##Project Information##");
                Console.WriteLine("Project ID: " + project.Id);
                Console.WriteLine("Project Title: " + project.Title);
                Console.WriteLine("Project Description: " + project.Description);
            }

        }

        private static void PrintTaskInformation(Task task)
        {
            if (task == null)
            {
                Console.WriteLine("That task didn't exist or something else happened..");
            }
            else
            {
                Console.WriteLine("##Task Informaltion##");
                Console.WriteLine("Task Id: " + task.Id);
                Console.WriteLine("Task Title" + task.Title);
            }
        }
    }
}
