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
        private static IProjectController _projectController;
        private static ITaskController _taskController;
        private static Project _project;

        #region Class Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            _projectController = new ProjectController();
            _taskController = new TaskController();

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            _project = new Project
            {
                Title = "UnitTestProject",
                Description = "Et fint lille unit test project",
                CreatedDate = DateTime.UtcNow,
                LastChange = DateTime.UtcNow,
                Done = false
            };
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
            Random rnd = new Random();
            string title1 = "UnitTest Creat remove 1 " + rnd.Next(999);
            string title2 = "UnitTest Creat remove 2 " + rnd.Next(999);
            string description2 = "UniTest Creat remove 2 desc" + rnd.Next(999);
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title1));
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title2, description2));
            Assert.AreEqual(true, _projectController.GetAllProjects().Length > 0);
            //Time To remove them again.. DUH DUH DUUUHHHHH!!!...
            Project project1 = _projectController.GetProject(title1).FirstOrDefault();
            Project project2 = _projectController.GetProject(title2).FirstOrDefault();
            Assert.IsNotNull(project1);
            Assert.IsNotNull(project1.Id);
            Assert.AreEqual(title1, project1.Title);
            Assert.AreEqual(ReturnValue.Success, _taskController.CreateTask("Fancy unit test task", "Det er en rimelig fancy task", Priority.Normal, project1.Id.Value));
            Assert.AreEqual(ReturnValue.Success, _taskController.CreateTask("Fancy unit test task2", "Det er en rimelig fancy ekstra task", Priority.Normal, project1.Id.Value));
            Assert.IsNotNull(project2);
            Assert.IsNotNull(project2.Id);
            Assert.AreEqual(title2, project2.Title);
            Assert.AreEqual(description2, project2.Description);
            Debug.Assert(project1.Id != null, "project1.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project1.Id.Value));
            Debug.Assert(project2.Id != null, "project2.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project2.Id.Value));
        }

        [TestMethod]
        public void SanitizerProjectTest() //TODO Awaiting changes from @SplintDK to unsanitize a sanitized string
        {
            Random rnd = new Random();
            string title = "UnitTest sanitize " + rnd.Next(999) + "'); DROP TABLE Project";
            string description = "UnitTest sanitize " + rnd.Next(999) + "'); DROP TABLE Project";
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title, description));
            Project project1 = _projectController.GetProject(title).FirstOrDefault();
            Assert.IsNotNull(project1);
            Assert.IsNotNull(project1.Id);
            Assert.AreEqual(description, project1.Description);
            Debug.Assert(project1.Id != null, "project1.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project1.Id.Value));

            string title2 = "UnitTest sanitize " + rnd.Next(999) + "'); DROP TABLE Project";
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title2));
            Project project2 = _projectController.GetProject(title2).FirstOrDefault();
            Assert.IsNotNull(project2);
            Assert.IsNotNull(project2.Id);
            Debug.Assert(project2.Id != null, "project2.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project2.Id.Value));
        }

        [TestMethod]
        public void TitleLengthTest()
        {
            Random rnd = new Random();
            string title1 = "UnitTest TitleLenght this title is far far far too long for me.." + rnd.Next(999);
            Assert.AreEqual(ReturnValue.StringLengthFail, _projectController.CreateProject(title1));

            string title2 = "UnitTest TitleLenght this is the max ok lenght " + rnd.Next(999);
            Assert.AreEqual(ReturnValue.Success, _projectController.CreateProject(title2));
            Project project2 = _projectController.GetProject(title2).FirstOrDefault();
            Assert.IsNotNull(project2);
            Assert.IsNotNull(project2.Id);
            Debug.Assert(project2.Id != null, "project2.Id != null");
            Assert.AreEqual(ReturnValue.Success, _projectController.RemoveProject(project2.Id.Value));
        }

        [TestMethod]
        public void GetProject()
        {
            Project[] projects = _projectController.GetProject("UnitTestProject");
            Project project = projects.FirstOrDefault();
            Debug.Assert(project != null, "project != null");
            Assert.AreEqual(_project.Title, project.Title);
            Assert.AreEqual(_project.Description, project.Description);
        }

        [TestMethod]
        public void UpdateProject()
        {
            //Generate something which probably doesnt excist in the database...
            Random rnd = new Random();
            string title = "UnitTest Update" + rnd.Next(999);
            string description = "UnitTest Update" + rnd.Next(999);
            const bool done = true;
            DateTime createdDate = DateTime.UtcNow;

            //Update the project with something hopefully random
            Debug.Assert(_project.Id != null, "_project.Id != null");
            Project project = _projectController.GetProject(_project.Id.Value);
            project.Title = title;
            project.Description = description;
            project.Done = done;
            project.CreatedDate = createdDate;
            Assert.AreEqual(ReturnValue.Success, _projectController.EditProject(project));
            Assert.AreEqual(_project.Id, project.Id);
            Debug.Assert(project.Id != null, "project.Id != null");
            Project projectAfterUpdate = _projectController.GetProject(project.Id.Value);
            Assert.AreEqual(project.Title, projectAfterUpdate.Title);
            Assert.AreEqual(project.Description, projectAfterUpdate.Description);
            Assert.AreEqual(project.Done, projectAfterUpdate.Done);
            Assert.AreEqual(project.CreatedDate, projectAfterUpdate.CreatedDate);
            Assert.AreEqual(project.Id, projectAfterUpdate.Id);
        }

        [TestMethod]
        public void GetTaskFromProject()
        {
            Task[] tasks = _projectController.GetTasksFromProject(1);
        }
    }
}
