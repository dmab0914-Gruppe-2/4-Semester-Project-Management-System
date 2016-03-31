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
        public int CreateTask(string name, string description, User assignedUser)
        {
            Models.Task task = new Models.Task
            {
                Title = name,
                Description = description,
                Status = TaskStatus.Assigned,
                AssignedUser = assignedUser,
                CreateDateTime = DateTime.UtcNow

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
        }

        public int CreateTask(string name, string description)
        {
            Models.Task task = new Models.Task
            {
                Title = name,
                Description = description,
                Status = TaskStatus.Unassigned,
                CreateDateTime = DateTime.UtcNow

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
        }

        public Models.Task[] GetTask(string name)
        {
            if (name.Length > 0)
                return container.GetTask(name).ToArray();
            else
                throw new KeyNotFoundException(name + "Does not excist!");
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
    }
}
