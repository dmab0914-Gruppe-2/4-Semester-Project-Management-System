using System;
using Logic.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;
using Task = Logic.Models.Task;

namespace Logic.Controllers
{
    public class TaskController : ITaskController
    {
        private Utility utility;
        private DbTask DbTask { get; set; }

        public TaskController()
        {
            DbTask = new DbTask();
            utility = new Utility();
        }

        public ReturnValue CreateTask(string title, string description, Priority priority, TaskStatus ts,
            int projectId, DateTime duedate)
        {
            if (utility.StringLength50(title))
            {
                Task task = new Task
                {
                    Title = title,
                    Description = description,
                    Priority = priority,
                    ProjectId = projectId,
                    DueDate = duedate,
                    Created = DateTime.UtcNow,
                    LastEdited = DateTime.UtcNow,
                    Status = ts
                };
                Task returnTask = (Task)utility.Sanitizer(task);
                return AddTask(returnTask);
            }
            return ReturnValue.StringLengthFail;

        }

        public ReturnValue CreateTask(string title, string description, Priority priority, TaskStatus ts, int projectId)
        {
            if (utility.StringLength50(title))
            {
                Task task = new Task
                {
                    Title = title,
                    Description = description,
                    Status = ts,
                    Created = DateTime.UtcNow,
                    Priority = priority,
                    ProjectId = projectId,
                    DueDate = DateTime.MaxValue,
                    LastEdited = DateTime.UtcNow

                };
                Task returnTask = (Task)utility.Sanitizer(task);
                return AddTask(returnTask);
            }
            return ReturnValue.StringLengthFail;
        }

        public ReturnValue CreateTask(string title, string description, Priority priority, int projectId)
        {
            if (utility.StringLength50(title))
            {
                Task task = new Task
                {
                    Title = title,
                    Description = description,
                    Status = TaskStatus.Unassigned,
                    Created = DateTime.UtcNow,
                    DueDate = DateTime.MaxValue,
                    LastEdited = DateTime.UtcNow,
                    Priority = priority,
                    ProjectId = projectId,

                };
                Task returnTask = (Task)utility.Sanitizer(task);
                return AddTask(returnTask);
            }
            return ReturnValue.StringLengthFail;
        }

        public ReturnValue CreateTask(string title, string description, Priority priority, int projectId, DateTime duedate)
        {
            if (utility.StringLength50(title))
            {
                Task task = new Task
                {
                    Title = title,
                    Description = description,
                    Priority = priority,
                    ProjectId = projectId,
                    DueDate = duedate,
                    LastEdited = DateTime.UtcNow,
                    Created = DateTime.UtcNow,
                    Status = TaskStatus.Unassigned
                };
                Task returnTask = (Task)utility.Sanitizer(task);
                return AddTask(returnTask);
            }
            return ReturnValue.StringLengthFail;
        }

        public Task[] GetTask(string title)
        {
            Task[] tasks;
            List<Task> sanitizedTasks = new List<Task>();
            if (title.Length > 0)
                 tasks = DbTask.GetTask(title).ToArray();
            else
                throw new KeyNotFoundException(title + " Does not excist!");
            foreach (Task task in tasks)
            {
                Task Sanitizedtask = (Task)utility.Desanitizer(task);
                sanitizedTasks.Add(Sanitizedtask);
            }
            return sanitizedTasks.ToArray();
        }

        public Task GetTask(int id)
        {
            Task task = (Task) utility.Desanitizer(DbTask.GetTask(id));
            return task;
        }


        public ReturnValue RemoveTask(int id)
        {
            DbTask.RemoveTask(id);
            if (DbTask.GetTask(id) == null)
                return ReturnValue.Success;
            return ReturnValue.Fail;
        }

        private ReturnValue AddTask(Task task)
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

        public ReturnValue UpdateTask(Task task)
        {
            if (task.Id != null && utility.StringLength50(task.Title))
            {
                Task returnTask = (Task)utility.Sanitizer(task);
                returnTask.LastEdited = DateTime.UtcNow;
                bool success = DbTask.UpdateTast(returnTask);
                if (!success)
                    return ReturnValue.Fail;
                Debug.Assert(task.Id != null, "task.Id != null");
                task = DbTask.GetTask((int)task.Id.Value);
                if (task.Title.Equals(returnTask.Title) &&
                    task.Description.Equals(returnTask.Description) &&
                    task.Status.Equals(returnTask.Status) &&
                    task.Priority.Equals(returnTask.Priority))
                    return ReturnValue.Success;
                return ReturnValue.Fail;
            }
            return ReturnValue.StringLengthFail;
        }

    }
}
