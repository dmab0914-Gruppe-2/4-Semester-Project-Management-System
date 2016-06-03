using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Logic.Models;
using Logic.Controllers;
using Web_UI.Models;
using Logic;
using Web_UI.Hubs;
using Microsoft.AspNet.SignalR;

namespace Web_UI.Controllers
{
    public class ProjectController : Controller
    {
        private IProjectController PC = new Logic.Controllers.ProjectController();
        //  private ProjectHub PH = new ProjectHub(); // DO NOT USE THIS, shit will break !

        // GET: Project // alt kode indhold i denne funktion er ligegyldigt 
        public ActionResult Index()
        {
            List<Project> result = PC.GetAllProjects().ToList();
            List<VMProject> projects = new List<VMProject>();
            foreach (Project p in result)
            {
                VMProject vp = new VMProject(p.Id, p.Title, p.Description, p.CreatedDate, p.LastChange, p.Done);
                projects.Add(vp);
            }
            return View(projects);
        }

        // GET: Get project list  // ONLY need for without signalR 
        public ActionResult ProjectList()
        {
            //List<Project> result = PC.GetAllProjects().ToList();
            //List<VMProject> projects = new List<VMProject>();
            //foreach (Project p in result)
            //{
            //    VMProject vp = new VMProject(p.Id, p.Title, p.Description, p.CreatedDate, p.LastChange, p.Done);
            //    projects.Add(vp);
            //}
            return View();
        }

        public List<VMProject> AllProjectsToList()
        {
            List<Project> result = PC.GetAllProjects().ToList();
            List<VMProject> projects = new List<VMProject>();
            foreach (Project p in result)
            {
                VMProject vp = new VMProject(p.Id, p.Title, p.Description, p.CreatedDate, p.LastChange, p.Done);
                projects.Add(vp);
            }

            return projects;
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string Title = Request.Form["Title"];
                    string Description = Request.Form["Description"];
                    if (Title != null && Description != null)
                    {
                        ReturnValue result = PC.CreateProject(Title, Description);
                        if (result == ReturnValue.Success)
                        {
                            var added = PC.GetProject(Title).ToList();
                            Project newP = added.Where(x => x.Title.Equals(Title) && x.Description.Equals(Description)).First();
                            VMProject vp = new VMProject(newP.Id, newP.Title, newP.Description, newP.CreatedDate, newP.LastChange, newP.Done);


                            var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                            context.Clients.All.addedProject(vp);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            Project p = PC.GetProject(id);
            VMProject vp = new VMProject(p.Id, p.Title, p.Description, p.CreatedDate, p.LastChange, p.Done);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(vp);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VMProject p = new VMProject();
                    p.Id = id;
                    p.Title = Request.Form["title"];
                    p.Description = Request.Form["description"];
                    p.Done = Convert.ToBoolean(Request.Form.GetValues("Done")[0]);

                    Project oldProject = PC.GetProject(id); // for signalR
                    VMProject vmOldProject = null;
                    if (oldProject != null)
                        vmOldProject = new VMProject(oldProject.Id, oldProject.Title, oldProject.Description, oldProject.CreatedDate, oldProject.LastChange, oldProject.Done);

                    Project update = new Project
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        Done = p.Done,
                        LastChange = DateTime.UtcNow.ToUniversalTime(),
                    };

                    ReturnValue result = PC.EditProject(update);

                    Project newProject = PC.GetProject(id); // for signalR
                    VMProject vmNewProject = null;
                    if (newProject != null)
                        vmNewProject = new VMProject(newProject.Id, newProject.Title, newProject.Description, newProject.CreatedDate, newProject.LastChange, newProject.Done);

                    if (result == ReturnValue.Success)
                    {
                        if(vmNewProject != null && vmOldProject != null && vmNewProject.Id == vmOldProject.Id)
                        {
                            var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                            context.Clients.All.updatedProject(vmOldProject, vmNewProject);
                        }
                        //return RedirectToAction("Index");
                        return RedirectToAction("ProjectList");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            Project p = PC.GetProject(id);
            VMProject vp = new VMProject { Id = p.Id, Title = p.Title, Description = p.Description, Done = p.Done };
            return View(vp);
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Project p = PC.GetProject(id);

                if (p.Id != null)
                {
                    ReturnValue rv = PC.RemoveProject(id);
                    VMProject vp = new VMProject(p.Id, p.Title, p.Description, p.CreatedDate, p.LastChange, p.Done);

                    if (rv == ReturnValue.Success)
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                        context.Clients.All.removeProject(vp);
                        return RedirectToAction("Index");
                    }
                    else
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // GET: Task to project
        public JsonResult getTasksForProject(int projectid)
        {
            List<Task> result = PC.GetTasksFromProject(projectid).ToList();
            List<VMTask> tasks = new List<VMTask>();

            foreach (Task t in result)
            {
                VMTask vt = new VMTask();
                vt.Id = t.Id;
                vt.Title = t.Title;
                vt.Description = t.Description;
                vt.Priority = TheirEnumExtensions.ToWebEnumPriority(t.Priority);
                vt.Status = TheirEnumExtensions.ToWebEnumTaskStatus(t.Status);
                vt.CreatedDate = t.Created;

                tasks.Add(vt);
            }
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }
    }
}
