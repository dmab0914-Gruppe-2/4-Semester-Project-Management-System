using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Logic;
using Logic.Controllers;
using Logic.DataAccess;
using Logic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class ProjectControllerTest
    {
        private static Logic.Controllers.IProjectController _projectController;
        private static Project _project;

        #region Class Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            
            _projectController = new ProjectController();
            _project = new Project
            {
                Title = "UnitTestProject",
                Description = "Et fint lille unit test project",
                CreatedDate = DateTime.UtcNow,
                LastChange = DateTime.UtcNow,
                Done = false
            };
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            Assert.IsTrue(new DbProject().AddProject(_project));
            Assert.IsNotNull(_project.Id);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Debug.Assert(_project.Id != null, "_project.Id != null");
            Assert.IsTrue(new DbProject().RemoveProject(_project.Id.Value));

        }

        [TestMethod]
        public void CreateAndRemoveProjectSuccess()
        {
            const string title1 = "Et virkeligt obskurt #navn!!!?! yea12389";
            const string title2 = "Mega fancy title13379! '); DROP TABLE Project";
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title1));
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title2));
            Assert.AreEqual(true, _projectController.GetAllProjects().Length > 0);
            //Time To remove them again.. DUH DUH DUUUHHHHH!!!...
            Project project1 = _projectController.GetProject(title1).FirstOrDefault();
            //Project project2 = _projectController.GetProject(title2).FirstOrDefault(); //TODO Awaiting changes from @SplintDK to unsanitize a sanitized string
            Assert.IsNotNull(project1);
            //Assert.IsNotNull(project2);
            Debug.Assert(project1.Id != null, "project1.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project1.Id.Value));
            //Debug.Assert(project2.Id != null, "project2.Id != null");
            //Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project2.Id.Value)); 


            #region old shit

            return;
            ReturnValue result = _projectController.CreateProject("Something", "Worked");
            if (result != ReturnValue.Success)
                Assert.Fail("ReturnValue is not success");
            _project = new Project { Title = "Something", Description = "Worked" };
            result = _projectController.CreateProject("Billy", "'); DROP TABLE Project");
            if (result != ReturnValue.Success)
                Assert.Fail("ReturnValue is not success");
            Project[] projects = _projectController.GetProject("Billy");
            Project project = projects.First();
            project.Description.Equals("''); DROP TABLE Project");
            #endregion
        }

        [TestMethod]
        public void CreateAndRemoveProjectFail()
        {
            //TODO test for too long title and such
        }

        [TestMethod]
        public void GetProject()
        {
            Project[] projects = _projectController.GetProject("UnitTestProject");
            Project project = projects.FirstOrDefault();
            if (!project.Description.Equals(_project.Description))
                Assert.Fail("Description mismatch");
            if (!project.Title.Equals(_project.Title))
                Assert.Fail("Title Mismatch");
        }

        [TestMethod]
        public void UpdateProject()
        {
            //Generate something which probably doesnt excist in the database...
            Random rnd = new Random();
            string title = rnd.Next().ToString();
            string description = rnd.Next().ToString();
            const bool done = true;
            DateTime createdDate = DateTime.UtcNow;

            //Update the project with something hopefully random
            _project.Title = title;
            _project.Description = description;
            _project.Done = done;
            _project.CreatedDate = DateTime.UtcNow;
            ReturnValue rt = _projectController.EditProject(_project);
            if (!rt.Equals(ReturnValue.Success))
                Assert.Fail("Project controller did not return success");

        }

        [TestMethod]
        public void RemoveProject()
        {

            Project[] projectCandidates = _projectController.GetProject("Something");
            foreach (Project project in projectCandidates)
            {
                int? id = project.Id;
                if (id == null)
                    Assert.Fail("Id is null after get method!");
                int Id = (int)id;
                _projectController.RemoveProject(Id);
            }
            projectCandidates = _projectController.GetProject("Something");
            if (projectCandidates.Length != 0)
                Assert.Fail("Still contains projects after removal!");


            //projectController.RemoveProject();
        }

        [TestMethod]
        public void GetTaskFromProject()
        {
            Task[] tasks = _projectController.GetTasksFromProject(1);
        }

        [TestMethod]
        public void AddTaskToMethod()
        {

        }

        #region Private test related operations methods

        #endregion
        #region Old stuff
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
        #endregion
    }
}
