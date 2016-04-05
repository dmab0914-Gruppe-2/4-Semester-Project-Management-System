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

        // GET: Project
        public ActionResult Index()
        {
            //TODO Fix add methods
            List<Task> tasks = new List<Task>();
            VMTask task1 = new VMTask { Title = "We gotta do this", Description = "Shit", Id = 2, Project = new VMProject(405), CreatedDate = DateTime.Now };
            VMTask task2 = new VMTask { Title = "Shit", Description = "We gotta do", Id = 1, Project = new VMProject(405), CreatedDate = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            List<VMProject> projects = new List<VMProject>();
            VMProject y = new VMProject { Description = "Awesome project", Id = 405, Title = "42", Tasks = tasks };
            VMProject x = new VMProject { Description = "hest project", Id = 406, Title = "hest", Tasks = null };
            projects.Add(y);
            projects.Add(x);

            return View(projects);
        }

        // GET: Get project list
        public ActionResult ProjectList()
        {
            //TODO Fix add methods
            List<Task> tasks = new List<Task>();
            VMTask task1 = new VMTask { Title = "We gotta do this", Description = "Shit", Id = 2, Project = new VMProject(405), CreatedDate = DateTime.Now };
            VMTask task2 = new VMTask { Title = "Shit", Description = "We gotta do", Id = 1, Project = new VMProject(405), CreatedDate = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            List<VMProject> projects = new List<VMProject>();
            VMProject y = new VMProject { Description = "Awesome project", Id = 405, Title = "42", Tasks = tasks };
            VMProject x = new VMProject { Description = "hest project", Id = 406, Title = "hest", Tasks = null };
            projects.Add(y);
            projects.Add(x);

            return View(projects);
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
           
            List<VMProject> projects = new List<VMProject>();
            VMProject y = new VMProject { Description = "Awesome project", Id = 405, Title = "42", Tasks = null, Done = true };
            VMProject x = new VMProject { Description = "hest project", Id = 406, Title = "hest", Tasks = null };
            projects.Add(y);
            projects.Add(x);

            VMProject result = projects.FirstOrDefault(u => u.Id == id);

            if (result == null)
                return HttpNotFound();

            ////Project project = container.GetAllProjects().FirstOrDefault(x => x.Id == id);
            //if (project == null)
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //// ViewBag.Title = new TextBox();
            return View(result);
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
            List<VMTask> result = new List<Task>();
            foreach(VMTask t in tasks)
            {
                if (projectid == t.Project.Id)
                    result.Add(t);
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}
