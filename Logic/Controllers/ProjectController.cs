using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.DataAccess;
using Logic.Models;
using Task = Logic.Models.Task;

namespace Logic.Controllers
{
    public class ProjectController : IProjectController
    {
        //private Container container = Container.Instance;
        private DbProject DbProject { get; set; }
        private DbTask DbTask { get; set; }
        private Utility utility = new Utility();

        //private string DATETIME_FORMAT = "YYYY-MM-DD hh:mm:ss.fff";
        //We're using the ISO 8601 Standard for DateTime. 
        //YYYY-MM-DD hh:mm:ss.mss
        //2016-05-25 22:15:55.000
        

        public ProjectController()
        {
            DbTask = new DbTask();
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
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                LastChange = DateTime.UtcNow.ToUniversalTime()
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
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                LastChange = DateTime.UtcNow.ToUniversalTime()
            };
            Project returnProject = (Project)utility.Sanitizer(project);
            return AddProject(returnProject);
            throw new NotImplementedException();
        }

        public ReturnValue CreateProject(string name)
        {
            Project project = new Project
            {
                Title = name,
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                LastChange = DateTime.UtcNow.ToUniversalTime()
            
            };
            Project returnProject = (Project)utility.Sanitizer(project);
            return AddProject(returnProject);
            throw new NotImplementedException();
        }

        public ReturnValue RemoveProject(int projectId)
        {
            Project project = DbProject.GetProject(projectId);
            if(project == null)
                return ReturnValue.Fail;
            DbProject.RemoveProject(projectId);
            project = DbProject.GetProject(projectId);
            if(project == null)
                return ReturnValue.Success;
            return ReturnValue.UnknownFail;
            //throw new NotImplementedException("Didnt finish");
        }

        /// <summary>
        /// Takes a project as parameter, then updates the given projects and assures the changes were successful. 
        /// </summary>
        /// <param name="project">The updated project, which MUST have the same id as the project which is to be edited.</param>
        /// <returns>Returnvalue according if the changes to project were successful</returns>
        public ReturnValue EditProject(Project project)
        {
            Project returnProject = (Project)utility.Sanitizer(project);
            DbProject.UpdateProject(returnProject);
            project = DbProject.GetProject((int) project.Id);
            if(project.Title.Equals(returnProject.Title) && 
                project.Description.Equals(returnProject.Description) &&
                project.Done.Equals(returnProject.Done) &&
                project.LastChange.Equals(returnProject.LastChange))
                return ReturnValue.Success;
            return ReturnValue.Fail;
            throw new NotImplementedException("TODO uncomment when function excists...");
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
            Project project = DbProject.GetProject(projectId);
            Task task = DbTask.GetTask(taskId);
            if (project == null || task == null)
            {
                throw new KeyNotFoundException("Project or Task does not excist!");
            }
            project.Tasks.Add(task);

            //todo change to dbaccess code...
            project.Tasks.Add(task);
            throw new NotImplementedException("Not done yet..");
        }

        public ReturnValue RemoveTaskFromProject(int taskId, int projectId)
        {
            Project project = DbProject.GetProject(projectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project with id: " + projectId + " does not excist!");
            }
            Task task = DbTask.GetTask(taskId);
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
            try
            {
                Project project = DbProject.GetProject(projectId);
                if (project == null)
                {
                    throw new KeyNotFoundException("Project does not excist!");
                }
                //List<Models.Task> tasks = new List<Task>();
                //foreach (Models.Task task in project.Tasks)
                //{
                //    tasks.Add(task);
                //}
                return DbTask.GetAllTasks().Where(x => x.ProjectId == projectId).ToArray();

            }
            catch (SqlException)
            {

                throw new Exception("Something Went wrong in the DB...");
            }

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

        
        //private DateTime ParseDateTime(DateTime dt)
        //{
        //    //DateTime parsedDateTime = DateTime.UtcNow;
        //    bool success = DateTime.TryParse(dt.ToString(), DATETIME_FORMAT, null, DateTimeStyles.None, out parsedDateTime);
        //    return new DateTime();
        //}
    }
}   
