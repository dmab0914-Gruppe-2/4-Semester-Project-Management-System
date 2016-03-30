using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Models
{
    /// <summary>
    /// TEST container class to store Task and Project. 
    /// </summary>
    public sealed class Container
    {
        private static Container instance = new Container();
        private List<Project> projects;
        private List<Task> tasks;

        public static Container Instance
        {
            get
            {
                return instance;
            }
        }

        private Container()
        {
            projects = new List<Project>();
            tasks = new List<Task>();
        }

        /// <summary>
        /// Adds a project to a singleton list of projects
        /// </summary>
        /// <param name="project"></param>
        /// <returns>0 if added, else returns 1</returns>
        public int AddProject(Project project)
        {
            projects.Add(project);
            if (projects.Contains(project))
                return 0;
            return 1;
        }

        /// <summary>
        /// Adds a task to a singleton list of tasks. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns>0 if added successfully, else returns 1</returns>
        public int AddTask(Task task)
        {
            tasks.Add(task);
            if (tasks.Contains(task))
                return 0;
            return 1;
        }

        public List<Project> GetAllProjects()
        {
            return projects;
        }
    }
}
