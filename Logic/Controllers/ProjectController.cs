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
        private DbProject DbProject { get; set; }
        private DbTask DbTask { get; set; }
        private Utility utility = new Utility();

        public ProjectController()
        {
            DbProject = new DbProject();
        }
        public ReturnValue CreateProject(string name, string description, User leaderUser)
        {
            Project project = new Project
            {
                Done = false,
                Description = description,
                LeaderUser = leaderUser,
                Title = name,
                CreatedDate = DateTime.UtcNow
            };
            Project returnProject = (Project)utility.Sanitizer(project);

            //container.AddProject(project);
            //TODO when dbaccess is done, make sure that the db is checked if the data has been added
            return AddProject(returnProject);        
        }

        public ReturnValue CreateProject(string name, string description)
        {
            
            Project project = new Project
            {
                Done = false,
                Title = name,
                Description = description,
                CreatedDate = DateTime.UtcNow
            };
            Project returnProject = (Project)utility.Sanitizer(project);
            return AddProject(returnProject);
            throw new NotImplementedException();
        }

        public ReturnValue CreateProject(string name)
        {
            Project project = new Project { Title = name };
            Project returnProject = (Project) utility.Sanitizer(project);
            return AddProject(returnProject);
            throw new NotImplementedException();
        }

        public Project[] GetProject(string name)
        {
            if (name.Length > 0)
                return DbProject.GetProject(name).ToArray();
            else
                throw new Exception("Title defined not found");
        }

        public Project GetProject(int id)
        {
            return DbProject.GetProject(id);
        }

        public ReturnValue AddTaskToProject(int taskId, int projectId)
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

        public ReturnValue RemoveTaskFromProject(int taskId, int projectId)
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
            return ReturnValue.Success;
        }



        public Project[] GetAllProjects()
        {
            //TODO change to DBaccess code
            return DbProject.GetAllProjects().ToArray();
            //return container.GetAllProjects().ToArray();
        }

        public Models.Task[] GetTasksFromProject(int projectId)
        {
            Project project = DbProject.GetProject(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project does not excist!");
            }
            return DbTask.GetAllTasks().Where(x => x.ProjectId == projectId).ToArray();
        }

        private ReturnValue AddProject(Project project)
        {
            try
            {
                if (DbProject.AddProject(project))
                    return ReturnValue.Success;
            }
            catch (Exception)
            {
                //todo when Jacob defines more errors... Do more work here...
                //todo remove project when Jacob fixes thingy..
                return ReturnValue.UnknownFail;
            }
            return ReturnValue.UnknownFail;


            //switch (container.AddProject(project))
            //{
            //    //Success
            //    case 0:
            //        return ReturnValue.Success;
            //    //Unsuccess
            //    case 1:
            //        //container.RemoveProject(project);
            //        return ReturnValue.Fail;
            //    //Error
            //    default:
            //        container.RemoveProject(project);
            //        return ReturnValue.UnknownFail;
            //}
        }
    }
}
