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
                Assert.Fail("ReturnValue is not success");
            result = projectController.CreateProject("Billy", "'); DROP TABLE Project");
            if (result != ReturnValue.Success)
                Assert.Fail("ReturnValue is not success");
            Project[] projects = projectController.GetProject("Billy");
            Project project = projects.First();
            project.Description.Equals("''); DROP TABLE Project");
        }

        [TestMethod]
        public void GetProject()
        {
            Project[] projects = projectController.GetProject("Something");
            //if()
        }

        [TestMethod]
        public void RemoveProject()
        {
            Project[] projectCandidates = projectController.GetProject("Something");
            foreach (Project project in projectCandidates)
            {
                int? id = project.Id;
                if (id == null)
                    Assert.Fail("Id is null after get method!");
                int Id = (int) id;
                projectController.RemoveProject(Id);
            }
            projectCandidates = projectController.GetProject("Something");
            if (projectCandidates.Length != 0)
                Assert.Fail("Still contains projects after removal!");
            //projectController.RemoveProject();
        }

        [TestMethod]
        public void GetTaskFromProject()
        {
            Task[] tasks = projectController.GetTasksFromProject(1);
        }

        [TestMethod]
        public void AddTaskToMethod()
        {

        }

    }
}
