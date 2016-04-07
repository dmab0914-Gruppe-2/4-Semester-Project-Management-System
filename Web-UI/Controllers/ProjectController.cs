using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Logic.Models;
using Logic.Controllers;
using Web_UI.Models;
using Logic;
using Logic.DataAccess;



namespace Web_UI.Controllers
{
    public class ProjectController : Controller
    {
        private IProjectController PC = new Logic.Controllers.ProjectController();

        // GET: Project
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

        // GET: Get project list
        public ActionResult ProjectList()
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
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }
                }
                // TODO: Add insert logic here

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
                    VMProject project = new VMProject();
                    project.Title = Request.Form["name"];
                    project.Description = Request.Form["description"];
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // TODO: Add update logic here

                return RedirectToAction("Index");
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
            VMProject vp = new VMProject { Id = p.Id };
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
               //TO DO

                // remove tasks from project 

                // then remove project

                return RedirectToAction("Index");
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

            foreach(Task t in result)
            {
                VMTask vt = new VMTask();
                vt.Id = t.Id;
                vt.Title = t.Title;
                vt.Description = t.Description;
                vt.Priority = TheirEnumExtensions.ToWebEnumPriority(t.Priority);
                vt.Status = TheirEnumExtensions.ToWebEnumTaskStatus(t.Status);
                vt.CreatedDate = t.Created;
                vt.LastChangedDate = t.LastEdited;
                vt.DueDate = t.DueDate;

                tasks.Add(vt);
            }
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }
    }
}
