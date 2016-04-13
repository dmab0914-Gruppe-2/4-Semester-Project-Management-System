using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
        TaskController taskController = new TaskController();

        //private string DATETIME_FORMAT = "YYYY-MM-DD hh:mm:ss.fff";
        //We're using the ISO 8601 Standard for DateTime. 
        //YYYY-MM-DD hh:mm:ss.mss
        //2016-05-25 22:15:55.000


        public ProjectController()
        {
            DbTask = new DbTask();
            DbProject = new DbProject();
        }

        public ReturnValue CreateProject(string title, string description, User leaderUser)
        {
            if (utility.StringLength50(title))
            {
                Project project = new Project
                {
                    Done = false,
                    Description = description,
                    LeaderUser = leaderUser,
                    Title = title,
                    CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                    LastChange = DateTime.UtcNow.ToUniversalTime()
                };
                Project returnProject = (Project)utility.Sanitizer(project);
                return AddProject(returnProject);
            }
            return ReturnValue.StringLengthFail;
        }

        public ReturnValue CreateProject(string title, string description)
        {
            if (utility.StringLength50(title))
            {
                Project project = new Project
                {
                    Done = false,
                    Title = title,
                    Description = description,
                    CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                    LastChange = DateTime.UtcNow.ToUniversalTime()
                };
                Project returnProject = (Project)utility.Sanitizer(project);
                return AddProject(returnProject);
            }
            return ReturnValue.StringLengthFail;
        }

        public ReturnValue CreateProject(string title)
        {
            if (utility.StringLength50(title))
            {
                Project project = new Project
                {
                    Title = title,
                    CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                    LastChange = DateTime.UtcNow.ToUniversalTime()

                };
                Project returnProject = (Project)utility.Sanitizer(project);
                return AddProject(returnProject);
            }
            return ReturnValue.StringLengthFail;

        }

        public ReturnValue RemoveProject(int projectId)
        {
            Project project = DbProject.GetProject(projectId);
            if (project == null)
                return ReturnValue.Fail;
            Task[] tasks = GetTasksFromProject(projectId);
            foreach (Task task in tasks)
            {
                ReturnValue rt = taskController.RemoveTask(task.Id.Value);
                if (rt == ReturnValue.Success) { continue; }
                if (rt == ReturnValue.Fail || rt == ReturnValue.UnknownFail)
                    return ReturnValue.Fail;
            }
            DbProject.RemoveProject(projectId);
            project = DbProject.GetProject(projectId);
            if (project == null)
                return ReturnValue.Success;
            return ReturnValue.UnknownFail;
        }

        /// <summary>
        /// Takes a project as parameter, then updates the given projects and assures the changes were successful. 
        /// </summary>
        /// <param name="project">The updated project, which MUST have the same id as the project which is to be edited.</param>
        /// <returns>Returnvalue according if the changes to project were successful</returns>
        public ReturnValue EditProject(Project project)
        {
            if (utility.StringLength50(project.Title))
            {
                Project returnProject = (Project)utility.Sanitizer(project);
                DbProject.UpdateProject(returnProject);
                Debug.Assert(project.Id != null, "project.Id != null");
                project = DbProject.GetProject((int)project.Id);
                if (project.Title.Equals(returnProject.Title) &&
                    project.Description.Equals(returnProject.Description) &&
                    project.Done.Equals(returnProject.Done) &&
                    project.LastChange.Equals(returnProject.LastChange))
                    return ReturnValue.Success;
                return ReturnValue.Fail;
            }
            return ReturnValue.StringLengthFail;
        }

        public Project[] GetProject(string title)
        {
            if (title.Length > 0)
                return DbProject.GetProject(title).ToArray();
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
                return DbTask.GetTasksByProject(projectId).ToArray();

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
