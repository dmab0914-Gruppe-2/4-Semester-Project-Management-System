using System;
using Logic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;

namespace Logic.Controllers
{
    public class TaskController : ITaskController
    {
        private Container container;
        private Utility utility;
        private DbTask DbTask { get; set; }

        public TaskController()
        {
            container = Container.Instance;
            DbTask = new DbTask();
            utility = new Utility();
        }

        public ReturnValue CreateTask(string title, string description, Priority priority, User assignedUser,
            int projectId, DateTime duedate)
        {
            Models.Task task = new Models.Task();
        }
        public ReturnValue CreateTask(string title, string description, Priority priority, User assignedUser, int projectId)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
                Description = description,
                Status = TaskStatus.Assigned,
                AssignedUser = assignedUser,
                Created = DateTime.UtcNow,
                Priority = priority,
                ProjectId = projectId,
                DueDate = DateTime.MaxValue,
                LastEdited = DateTime.UtcNow

            };
            Models.Task returnTask = (Models.Task)utility.Sanitizer(task);
            return AddTask(returnTask);
        }

        public ReturnValue CreateTask(string title, string description, Priority priority, int projectId)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
                Description = description,
                Status = TaskStatus.Assigned,
                Created = DateTime.UtcNow,
                DueDate = DateTime.MaxValue,
                LastEdited = DateTime.UtcNow,
                Priority = priority,
                ProjectId = projectId

            };
            Models.Task returnTask = (Models.Task)utility.Sanitizer(task);
            return AddTask(returnTask);
        }

        public Models.Task[] GetTask(string title)
        {
            if (title.Length > 0)
                return container.GetTask(title).ToArray();
            else
                throw new KeyNotFoundException(title + " Does not excist!");
        }

        public Models.Task GetTask(int id)
        {
            Models.Task task = container.GetTask(id);
            if (task == null)
            {
                throw new KeyNotFoundException(id.ToString() + " Does not excist!");
            }
            return task;
        }


        public ReturnValue RemoveTask(int id)
        {
            DbTask.RemoveTask(id);
            if(DbTask.GetTask(id) == null)
                return ReturnValue.Success;
            return ReturnValue.Fail;
        }

        private ReturnValue AddTask(Models.Task task)
        {
            switch (DbTask.AddTask(task))
            {
                case true:
                    return ReturnValue.Success;
                case false:
                    return ReturnValue.Fail;
                default:
                    return ReturnValue.UnknownFail;
            }
            
            //switch (container.AddTask(task))
            //{
            //    //Success add
            //    case 0:
            //        return ReturnValue.Success;
            //    //Unsuccess add 
            //    case 1:
            //        return ReturnValue.Fail;
            //    //Fail
            //    default:
            //        return ReturnValue.UnknownFail;
            //}
        }
    }
}
