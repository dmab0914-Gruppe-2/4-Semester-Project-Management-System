using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;
using Logic.Models;

namespace DataAccessConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Here we go!");
            TestGetProject();
            Console.ReadLine();
        }

        private static void TestGetProject()
        {
            Console.WriteLine("##Testing retrieval of 1'st project: Project 1##");
            PrintProjectInformation(DbProject.GetProject(1));
            Console.WriteLine("##Testing retrieval of all projects..##");
            List<Project> projects = DbProject.GetAllProjects();
            foreach (var project in projects)
            {
                PrintProjectInformation(project);
            }
            //sConsole.WriteLine(DateTime.UtcNow);

        }

        private static void PrintProjectInformation(Project project)
        {
            Console.WriteLine("##Project Information##");
            Console.WriteLine("Project ID: " + project.Id);
            Console.WriteLine("Project Title: " + project.Title);
            Console.WriteLine("Project Description: " + project.Description);
        }
    }
}
