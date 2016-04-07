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
        TaskController taskController = new TaskController();
        private int taskId;
        [TestMethod]
        public void CreateTask()
        {
            Task task = new Task
            {
                Priority = Priority.Normal,
                Created = DateTime.UtcNow,
                Description = "Something",
                Title = "Worked",
                Status = TaskStatus.Unassigned,
            };
            ReturnValue result = taskController.CreateTask(task.Title, task.Description, task.Priority);
            if(result != ReturnValue.Success)
                Assert.Fail("Failed at something when creating");


        }

        [TestMethod]
        public void GetTask()
        {
            Task[] tasks = taskController.GetTask("Worked");
            if (!tasks.Contains(tasks.FirstOrDefault(
                x => x.Title.Equals("Worked") &&
                     x.Description.Equals("Something") &&
                     x.Status.Equals(TaskStatus.Unassigned))))
                Assert.Fail("Does not match the task created in CreateTask test");
            Task task = tasks.First(x => x.Title.Equals("Worked"));
            taskId = (int)task.Id;
        }

        [TestCleanup]
        public void CleanupCrew()
        {
            taskController.RemoveTask(taskId);
        }
    }
}
