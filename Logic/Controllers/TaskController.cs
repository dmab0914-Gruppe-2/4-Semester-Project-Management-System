﻿using System;
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
        public int CreateTask(string title, string description, Priority priority, User assignedUser)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
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

        public int CreateTask(string title, string description, Priority priority)
        {
            Models.Task task = new Models.Task
            {
                Title = title,
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

        public int CreateTask(string title, Priority priority)
        {
            throw new NotImplementedException();
        }

        public int CreateTask(string title)
        {
            throw new NotImplementedException();
        }

        public Models.Task[] GetTask(string title)
        {
            if (title.Length > 0)
                return container.GetTask(title).ToArray();
            else
                throw new KeyNotFoundException(title + "Does not excist!");
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


        /// <summary>
        /// Checks if theres an attempt for SQL injection
        /// </summary>
        /// <param name="input"></param>
        /// <returns>false if no hazzards, true if test is positive for hazzards.</returns>
        public bool CheckInjection(string input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If a string contains hazzardous characters 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string which is safe to input into a database</returns>
        public string CorrectInjection(string input)
        {
            throw new NotImplementedException();
        }
    }
}
