using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Logic.DataAccess;
using Logic.Models;
using Task = Logic.Models.Task;

namespace Logic.Controllers
{
    public class ProjectController : IProjectController
    {
        private DbProject DbProject { get; set; }
        private DbTask DbTask { get; set; }
        private Utility utility = new Utility();
        TaskController taskController = new TaskController();

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

        public Project[] GetAllProjects()
        {
            return DbProject.GetAllProjects().ToArray();
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
        }
    }
}
