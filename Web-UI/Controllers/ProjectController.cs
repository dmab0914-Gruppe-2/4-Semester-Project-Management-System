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

                    string name = Request.Form["name"];
                    string description = Request.Form["description"];
                    if (name != null && description != null)
                    {
                        ReturnValue result = PC.CreateProject(name, description);
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
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Task to project
        public JsonResult getTasksForProject(int projectid)
        {
            List<VMTask> tasks = new List<VMTask>();
            VMTask task1 = new VMTask { Title = "We gotta do this", Description = "Shit", Id = 2, Project = new VMProject(405), CreatedDate = DateTime.Now };
            VMTask task2 = new VMTask { Title = "Shit", Description = "We gotta do", Id = 1, Project = new VMProject(405), CreatedDate = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            List<VMTask> result = new List<VMTask>();
            foreach (VMTask t in tasks)
            {
                if (projectid == t.Project.Id)
                    result.Add(t);
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}
