using System;
using System.Linq;
using Logic;
using Logic.Models;
using Logic.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class TaskControllerTest
    {
        TaskController tc = new TaskController();
        private int _taskId;
        [TestMethod]
        public void CreateTask()
        {
            Task task = new Task
            {
                //Id
                Title = "Worked",
                //Assigned to
                Description = "Something",
                Priority = Priority.Normal,
                Created = DateTime.UtcNow,
                ProjectId = 1,
                Status = TaskStatus.Unassigned,
            };
            ReturnValue result = tc.CreateTask(task.Title, task.Description, task.Priority, task.ProjectId);
            if (result != ReturnValue.Success)
                Assert.Fail("Failed at something when creating");


        }

        [TestMethod]
        public void GetTaskName()
        {
            Task[] tasks = tc.GetTask("Worked");
            Task task = tasks.FirstOrDefault();
            if(task == null)
                Assert.Fail("Task is null");
            if(!task.Title.Equals("Worked"))
                Assert.Fail("Title not equal to created task test");
            if(!task.Description.Equals("Something"))
                Assert.Fail("Description is not equal to create task test");

            //if (!tasks.Contains(tasks.FirstOrDefault(
            //    x => x.Title.Equals("Worked") &&
            //         x.Description.Equals("Something") &&
            //         x.Status.Equals(TaskStatus.Unassigned))))
            //    Assert.Fail("Does not match the task created in CreateTask test");
            //Task task = tasks.First(x => x.Title.Equals("Worked"));
            if (task.Id == null)
                Assert.Fail("Returned task id is null!");
            _taskId = (int)task.Id;
        }

        [TestMethod]
        public void GetTaskId()
        {
            //Works as long as the db scripts havent changed the properties 
            //Insert Into Task("Title", "AssignedTo", "Description", "Priority", "Status", "Created", "DueDate", "LastEdited", "ProjectId") Values
            //('My task', 'Jens', 'we need to get this stuff done fast', 1, 1, '2013-05-25 22:15:55.000', '2016-05-26 15:15:55.000', '2014-05-26 19:15:55.000', 0),
            //('My task', 'Jens', 'we need to get this stuff done fast', 0, 1, '2016-05-25 22:15:55.000', '2016-05-26 15:15:55.000', '2016-05-26 19:15:55.000', 1),
            //('DO THIS !', 'Hans', 'Man kan fremad se, at de har været udset til at låse, at der skal dannes par af ligheder.', 1, 2, '2016-06-25 22:15:55.000', '2016-06-26 15:15:55.000', '2016-07-26 19:15:55.000', 1);

            Task task = tc.GetTask(1);
            if (!task.Title.Equals("My task"))
                Assert.Fail("Name not equal to db script");
            if(!task.Description.Equals("we need to get this stuff done fast"))
                Assert.Fail("Description is not equal to db script ");
            if(!task.Priority.Equals(Priority.Unassigned))
                Assert.Fail("Priority does not match");
            if(!task.Status.Equals(TaskStatus.Assigned))
                Assert.Fail("Task status does not match db script!");
        }

        [TestCleanup]
        public void CleanupCrew()
        {
            tc.RemoveTask(_taskId);
        }
    }
}
