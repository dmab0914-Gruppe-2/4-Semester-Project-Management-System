using System;
using Logic.Controllers;
using Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UtilityTest
    {
        Utility utility = new Utility();
        [TestMethod]
        public void TestSanitizer()
        {
            string title = "'CREATE TABLE YouDoneGoofed(id int IDENTITY(1,1) NOT NULL, Primary key id)";
            Project project = new Project
            {
                Description = "'DROP DATABASE PMS", 
                Id = null, 
                CreatedDate = DateTime.UtcNow, 
                LastChange = DateTime.UtcNow,
                Title = title
            };

            //Forgot how to cast objects... this is how...
            Project returnProject = (Project)utility.Sanitizer(project);
            if (returnProject.Title.Equals(title))
            {
                Assert.Fail();
            }

        }
    }
}
