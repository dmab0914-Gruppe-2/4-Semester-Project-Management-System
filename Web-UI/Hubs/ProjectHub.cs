using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Web_UI.Controllers;
using Web_UI.Models;

namespace Web_UI.Hubs
{
    public class ProjectHub : Hub
    {
        private ProjectController pc = new ProjectController();
        public void GetProjects()
        {
            var projects = pc.AllProjectsToList();

            Clients.Caller.populateProjects(projects.ToArray());
        }
        public void GetProjectsList()
        {
            var projects = pc.AllProjectsToList();

            Clients.Caller.populateProjects(projects.ToArray());
        }

        //public void RemoveProject(VMProject vmp)
        //{
            
        //    Clients.All.removeProject(vmp);
        //}

        //public void UpdateProject(VMProject oldProject, VMProject newProject)
        //{
        //    var projects = pc.AllProjectsToList();

        //    Clients.All.updatedProject(oldProject, newProject, projects);
        //}

        public void UpdateTask()
        {
            Clients.All.changedTask();
        }
    }
}