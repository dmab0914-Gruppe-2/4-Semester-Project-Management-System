using System;
using System.Collections.Generic;
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
            Project project = DBProject.GetProject(1);
            Console.WriteLine("##Project 1##");
            //Console.WriteLine("Project ID: " + project.Id);
            //Console.WriteLine("Project Title: " + project.Name);
            //Console.WriteLine("Project Description: " + project.Description);
        }
    }
}
