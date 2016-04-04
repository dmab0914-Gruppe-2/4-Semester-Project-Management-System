using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Controllers;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a number to enter a test:");
            Console.WriteLine("1. String Sanitizer");
            Console.WriteLine("2. Object Sanitizer");

            string input = Console.ReadLine().ToLower();
            if (input.Equals("1"))
            {
                StringSanitizerTest();
            }
            if (input.Equals("2"))
            {
                ObjectSanitizerTest();
            }
        }

        static void  StringSanitizerTest()
        {
            bool run = true;
            while (run)
            {
                Utility utility = new Utility();
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
            
        }
    }
}
