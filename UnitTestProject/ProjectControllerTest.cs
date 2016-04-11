using System;
using System.Collections.Generic;
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
        private Project _project;

        [TestMethod]
        public void CreateProject()
        {
            ReturnValue result = projectController.CreateProject("Something", "Worked");
            if (result != ReturnValue.Success)
                Assert.Fail("ReturnValue is not success");
            _project = new Project {Title = "Something", Description = "Worked"};
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
            _project = projects.FirstOrDefault();
            if (!_project.Description.Equals("Worked"))
                Assert.Fail("Description mismatch");
            if(!_project.Title.Equals("Something"))
                Assert.Fail("Title Mismatch");
        }

        [TestMethod]
        public void UpdateProject()
        {
            //Generate something which probably doesnt excist in the database...
            Random rnd = new Random();
            int rndNumber = rnd.Next();

            //Update the project with something hopefully random
            Project[] projects = projectController.GetProject("Something");
            _project = projects.FirstOrDefault();
            _project.Title = rndNumber.ToString();
            ReturnValue rt = projectController.EditProject(_project);
            if(!rt.Equals(ReturnValue.Success))
                Assert.Fail("Project controller did not return success");

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
                int Id = (int)id;
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

        //[TestCleanup]
        //public void Cleanup()
        //{
        //    //Something projects removal
        //    Project[] projectCandidates = projectController.GetProject("Something");
        //    foreach (Project project in projectCandidates)
        //    {
        //        int? id = project.Id;
        //        if (id == null)
        //            Assert.Fail("Id is null after get method!");
        //        int Id = (int)id;
        //        projectController.RemoveProject(Id);
        //    }

        //    //Billy projects removal
        //    projectCandidates = projectController.GetProject("Billy");
        //    foreach (Project project in projectCandidates)
        //    {
        //        int? id = project.Id;
        //        if (id == null)
        //            Assert.Fail("Id is null after get method!");
        //        int Id = (int)id;
        //        projectController.RemoveProject(Id);
        //    }

        //    //Edited random projects removal
        //    projectCandidates = projectController.GetAllProjects();
        //    List<Project> projects = new List<Project>();   
        //    foreach (Project project in projectCandidates)
        //    {
        //        if(project.Description.Equals("Worked"))
        //            projects.Add(project);
        //    }
        //    foreach (Project project in projects)
        //    {
        //        projectController.RemoveProject((int) project.Id);
        //    }
        //}

    }
}
