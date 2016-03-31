﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Controllers
{
    public class ProjectController : IProjectController
    {
        private Container container = Container.Instance;
        public int CreateProject(string name, string description, User leaderUser)
        {
            if (name.Length == 0)
            {
                throw new NotImplementedException();
            }
            if (description.Length == 0)
            {
                throw new NotImplementedException();
            }
            if (leaderUser == null)
            {
                throw new NullReferenceException("No Leader user defined!");
            }
            Project project = new Project
            {
                Description = description,
                LeaderUser = leaderUser,
                Name = name
            };
            container.AddProject(project);
            throw new NotImplementedException();
        }

        public int CreateProject(string name, string description)
        {
            if (name.Length == 0)
            {
                throw new NotImplementedException("No name defined!");
            }
            if (description.Length == 0)
            {
                throw new NotImplementedException("No description defined");
            }
            Project project = new Project
            {
                Name = name,
                Description = description

            };
            switch (container.AddProject(project))
            {
                //Success
                case 0:
                    return 0;
                //Unsuccess
                case 1:
                    //container.RemoveProject(project);
                    return 1;
                //Error
                default:
                    container.RemoveProject(project);
                    return -1;
            }
            throw new NotImplementedException();
        }

        public int CreateProject(string name)
        {
            Project project = new Project { Name = name };
            switch (container.AddProject(project))
            {
                //Success
                case 0:
                    return 0;
                //Unsuccess
                case 1:
                    return 1;
                //Fail
                default:
                    container.RemoveProject(project);
                    return -1;
            }
            throw new NotImplementedException();
        }

        public Models.Project[] GetProject(string name)
        {
            if (name.Length > 0)
                return container.GetProject(name);
            else
                throw new Exception("No name defined");
        }

        public Models.Project GetProject(int id)
        {
            return container.GetProject(id);
        }

        public int AddTaskToProject(int taskId, int projectId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTaskFromProject(int taskId, int projectId)
        {
            Project project = container.GetProject(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project with id: " + projectId + " does not excist!");
            }
            Models.Task task = container.GetTask(taskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Task with id: " + taskId + " does not excist!");
            }
            // TODO when DBaccess is done, rewrite this to use dbaccess
            project.Tasks.Add(task);
            return 0;
        }

    }
}