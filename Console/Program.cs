using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logic.Controllers;
using Logic.Models;

namespace ConsoleApp
{
    class Program
    {
        private static Utility utility;
        static void Main(string[] args)
        {
            utility = new Utility();
        a:  
            Console.WriteLine("Please enter a number to enter a test:");
            Console.WriteLine("1. String Sanitizer");
            Console.WriteLine("2. Object Sanitizer");
            Console.WriteLine("3. Get Projects from Task");
            Console.WriteLine("4. Sanitize any object");
            
            string input = Console.ReadLine().ToLower();
            if (input.Equals("1"))
            {
                StringSanitizerTest();
            }
            if (input.Equals("2"))
            {
                ObjectSanitizerTest();
            }
            if (input.Equals("3"))
            {
                GetProjectsFromTaskTest();
            }
            if (input.Equals("4"))
            {
                AnyObjectSanitizer();
            }
            goto a;
        }

        private static void AnyObjectSanitizer()
        {
            //Set variables
            Console.WriteLine("Please enter the following values:");
            Console.WriteLine("Project name: ");
            string pname = Console.ReadLine();
            Console.WriteLine("Description: ");
            string pdesc = Console.ReadLine();

            //Create project
            Project project = new Project
            {
                Title = pname,
                Description = pdesc,
                CreatedDate = DateTime.UtcNow,
                LastChange = DateTime.UtcNow,
                Done = false,
            };

            //Sanitize code
            foreach (PropertyInfo p in project.GetType().GetProperties())
            {
                Console.WriteLine("Before");
                Console.WriteLine("\t{0} - {1}", p.Name, p.GetValue(project, null));
                if (p.PropertyType == typeof (string))
                {
                    p.SetValue(project, "I am a string..");
                }
                Console.WriteLine("After");
                Console.WriteLine("\t{0} - {1}", p.Name, p.GetValue(project, null));
            }
            Console.ReadLine();
            
        }

        private static void GetProjectsFromTaskTest()
        {
            throw new NotImplementedException();


        }

        static void  StringSanitizerTest()
        {
            bool run = true;
            while (run)
            {
                
                Console.WriteLine("Please enter a string for the sanitizer");
                string input = Console.ReadLine();
                string output = utility.Sanitizer(input);
                Console.WriteLine("Returned from sanitizer: {0}", output);
                Console.WriteLine("Try again? (Y/n)");
                string proceed = Console.ReadLine();
                proceed = proceed.ToLower();
                if (proceed.Equals("n"))
                {
                    run = false;
                }
                Console.WriteLine("Continueing ");
            }
        }

        static void ObjectSanitizerTest()
        {
            bool run = true;
            while (run)
            {
                Console.WriteLine("Testing with the project class");
                Console.Write("Project name\t\t:");
                string name = Console.ReadLine();
                Console.Write("Project Description\t:");
                string description = Console.ReadLine();

                Project project = new Project
                {
                    Title = name,
                    Description = description
                };
                Project returnProject = new Project();
                
                returnProject = (Project)utility.Sanitizer(project);
                Console.WriteLine("Return values:");
                Console.WriteLine("\tProject title\t:{0}", returnProject.Title);
                Console.WriteLine("\tProject desc\t:{0}", returnProject.Description);
                Console.WriteLine("Continue? (Y/n)");
                string proceed = Console.ReadLine().ToLower();
                if (proceed.Equals("n"))
                {
                    run = false;
                }
            }
        }
    }
}
