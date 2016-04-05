using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;
using Logic.Models;

namespace Logic.Controllers
{
    public class ProjectController : IProjectController
    {
        private Container container = Container.Instance;
        private Utility utility = new Utility();
        private DbProject dbProject = new DbProject();
        public int CreateProject(string name, string description, User leaderUser)
        {
            Project project = new Project
            {
                Description = description,
                LeaderUser = leaderUser,
                Title = name
            };
            Project returnProject = (Project)utility.Sanitizer(project);

            container.AddProject(project);
            //TODO when dbaccess is done, make sure that the db is checked if the data has been added
            switch (container.AddProject(returnProject))
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
        }

        public int CreateProject(string name, string description)
        {
            
            Project project = new Project
            {
                Title = name,
                Description = description

            };
            Project returnProject = (Project)utility.Sanitizer(project);
            switch (container.AddProject(returnProject))
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
            Project project = new Project { Title = name };
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
            Project project = container.GetProject(projectId);
            Models.Task task = container.GetTask(taskId);
            if (project == null || task == null)
            {
                throw new KeyNotFoundException("Project or Task does not excist!");
            }
            project.Tasks.Add(task);
            
            //todo change to dbaccess code...
            project.Tasks.Add(task);
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
            project.Tasks.Remove(task);
            return 0;
        }



        public Project[] GetAllProjects()
        {
            //TODO change to DBaccess code
            return DbProject.GetAllProjects().ToArray();
            //return container.GetAllProjects().ToArray();
        }
    }
}
