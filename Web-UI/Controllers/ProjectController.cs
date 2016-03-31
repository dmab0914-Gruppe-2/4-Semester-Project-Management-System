using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Logic.Models;
using Web_UI.Models;
using Project = Web_UI.Models.VMProject;
using Task = Web_UI.Models.VMTask;

namespace Web_UI.Controllers
{
    
    

    public class ProjectController : Controller
    {
        //TEMP DATA ACCESS
        private Container container = Container.Instance;
        //private List<Project> projects = new List<Project>();
        private int z;
        private int taskid;

        // GET: Project
        public ActionResult Index()
        {
            //Project y = new Project { Description = "Awesome project", Id = z++, Name = "42" };
            //projects.Add(y);

            //TODO Fix add methods
            List<Task> tasks = new List<Task>();
            VMTask task1 = new Task {Title = "We gotta do this", Description = "Shit", Id = taskid++, ProjectId = 404, Timestamp = DateTime.Now};
            VMTask task2 = new Task { Title = "Shit", Description = "We gotta do", Id = taskid++, ProjectId = 405, Timestamp = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            VMProject y = new Project { Description = "Awesome project", Id = z, Name = "42", Tasks = tasks};

            //container.AddProject(y);

            //VMProject project = new VMProject();
            //project.Projectname = "name";

            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Console.WriteLine("Id is equal to null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //todo Change to impliment data access
            //Project project = container.GetAllProjects().FirstOrDefault(x => x.Id == id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
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
                        //todo insert dbinsert here....
                        //container.AddProject(new Project
                        //{
                        //    Name = name,
                        //    Description = description
                        //});
                        return RedirectToAction("Index");
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

            ////Project project = container.GetAllProjects().FirstOrDefault(x => x.Id == id);
            //if (project == null)
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //// ViewBag.Title = new TextBox();
            return View();
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VMProject project = new Project();
                    project.Name = Request.Form["name"];
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
    }
}
