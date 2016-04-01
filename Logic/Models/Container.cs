using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Models
{
    /// <summary>
    /// TEST container class to store Task and Project. When data access is finished, replace this class.
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

        public int RemoveProject(Project project)
        {
            if (projects.Remove(project))
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        public int RemoveProject(int id)
        {
            projects.Remove(projects.First(x => x.Id == id));
            return 1;
        }

        public void RemoveProject(string name)
        {
            projects.Remove(projects.LastOrDefault(x => x.Title.Equals(name)));


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

        public Task GetTask(int id)
        {
            return tasks.First(x => x.Id == id);
        }
        public Task[] GetTask(string title)
        {
            return tasks.FindAll(x => x.Title == title).ToArray();
        }

        public Project GetProject(int id)
        {
            return projects.FirstOrDefault(x => x.Id == id);
        }

        public Project[] GetProject(string name)
        {
            return projects.FindAll(x => x.Title == name).ToArray();
        }
    }
}
