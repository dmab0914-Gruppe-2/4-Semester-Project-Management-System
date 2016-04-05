using System;
using System.Linq;
using Logic;
using Logic.Controllers;
using Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class ProjectControllerTest
    {
        private Logic.Controllers.IProjectController projectController = new ProjectController();

        [TestMethod]
        public void CreateProject()
        {
            ReturnValue result = projectController.CreateProject("Something", "Worked");
            if (result != ReturnValue.Success)
                Assert.Fail();
            result = projectController.CreateProject("Billy", "'); DROP TABLE Project");
            if (result != ReturnValue.Success)
                Assert.Fail();
            Project[] projects = projectController.GetProject("Billy");
            Project project = projects.First();
            project.Description.Equals("''); DROP TABLE Project");
        }

        [TestMethod]
        public void GetProject()
        {

        }

        [TestMethod]
        public void RemoveProject()
        {

        }

        [TestMethod]
        public void AddTaskToMethod()
        {

        }

    }
}
