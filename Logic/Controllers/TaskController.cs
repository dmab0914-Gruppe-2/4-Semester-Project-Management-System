using System;
using Logic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Controllers
{
    public class TaskController : ITaskController
    {
        private Container container = Container.Instance;
        private Utility utility;
        public int CreateTask(string title, string description, Priority priority, User assignedUser)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
                Description = description,
                Status = TaskStatus.Assigned,
                AssignedUser = assignedUser,
                CreateDateTime = DateTime.UtcNow,
                Priority = priority

            };
            Models.Task returnTask = (Models.Task)utility.Sanitizer(task);
            switch (container.AddTask(returnTask))
            {
                //Success add
                case 0:
                    return 0;
                //Unsuccess add 
                case 1:
                    return 1;
                //Fail
                default:
                    return -1;
            }
        }

        public int CreateTask(string title, string description, Priority priority)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
                Description = description,
                Status = TaskStatus.Unassigned,
                CreateDateTime = DateTime.UtcNow,
                Priority = priority

            };
            Models.Task returnTask = (Models.Task)utility.Sanitizer(task);
            switch (container.AddTask(returnTask))
            {
                //Success add
                case 0:
                    return 0;
                //Unsuccess add 
                case 1:
                    return 1;
                //Fail
                default:
                    return -1;
            }
        }

        public int CreateTask(string title, Priority priority)
        {
            Models.Task task = new Models.Task
            {
                Title = title, 
                Priority = priority
            };
            switch (container.AddTask(task))
            {
                //Success add
                case 0:
                    return 0;
                //Unsuccess add 
                case 1:
                    return 1;
                //Fail
                default:
                    return -1;
            }
            throw new NotImplementedException();
        }

        public int CreateTask(string title)
        {
            Models.Task task = new Models.Task {Title = title};
            Models.Task returnTask = (Models.Task)utility.Sanitizer(task);
            switch (container.AddTask(returnTask))
            {
                //Success add
                case 0:
                    return 0;
                //Unsuccess add 
                case 1:
                    return 1;
                //Fail
                default:
                    return -1;
            }
            throw new NotImplementedException();
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


        public int RemoveTask(int id)
        {
            throw new NotImplementedException();
        }



        public bool CheckInjection(string input)
        {
            throw new NotImplementedException();
        }

        public string CorrectInjection(string input)
        {
            throw new NotImplementedException();
        }
    }
}
