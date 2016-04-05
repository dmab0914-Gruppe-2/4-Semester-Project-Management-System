using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using Logic.Models;
using Web_UI.Models;
//using Web_UI.Models;

namespace Web_UI.Controllers
{
    public class TaskController : Controller
    {
        //private Container container = Container.Instance;
        private List<VMTask> Tasks = new List<VMTask>();
        private int y;
        // GET: Task
        public ActionResult Index()
        {
            Models.VMTask task = new Models.VMTask
            {
                Id = y++,
                Description = "Do the following........",
                CreatedDate = DateTime.UtcNow,
                //Status = TaskStatus.Assigned,
                Title = "Alright people, wake up!"
            };
            Tasks.Add(task);
            return View(Tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            List<VMTask> tasks = new List<VMTask>();
            VMTask task1 = new VMTask { Title = "We gotta do this", Description = "Shit", Status = Models.Enums.Status.InProgress, Id = 2, Project = new VMProject(405), CreatedDate = DateTime.Now };
            VMTask task2 = new VMTask { Title = "Shit", Description = "We gotta do",Status = Models.Enums.Status.Done, Id = 1, Project = new VMProject(405), CreatedDate = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            List<VMProject> projects = new List<VMProject>();
            VMProject y = new VMProject { Description = "Awesome project", Id = 405, Title = "42", Tasks = tasks };
            VMProject x = new VMProject { Description = "hest project", Id = 406, Title = "hest", Tasks = null };
            projects.Add(y);
            projects.Add(x);

           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMTask task = tasks.FirstOrDefault(u => u.Id == id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string title = Request.Form["title"];
                    string description = Request.Form["description"];
                    //DateTime timestamp = DateTime.UtcNow;


                    if (title != null && description != null)
                    {
                        VMTask task = new VMTask
                        {
                            Title = title,
                            Description = description,
                            CreatedDate = DateTime.UtcNow,

                        };
                        //TODO insert dbinsert here....
                        //if (container.AddTask(task) == 0)
                        //{
                        //    return RedirectToAction("Index");
                        //}
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            List<VMTask> tasks = new List<VMTask>();
            VMTask task1 = new VMTask { Title = "We gotta do this", Description = "Shit", Id = 2, Project = new VMProject(405), CreatedDate = DateTime.Now, LastChangedDate = DateTime.Now };
            VMTask task2 = new VMTask { Title = "Shit", Description = "We gotta do", Id = 1, Project = new VMProject(405), CreatedDate = DateTime.Now };
            tasks.Add(task1);
            tasks.Add(task2);
            List<VMProject> projects = new List<VMProject>();
            VMProject y = new VMProject { Description = "Awesome project", Id = 405, Title = "42", Tasks = tasks };
            VMProject x = new VMProject { Description = "hest project", Id = 406, Title = "hest", Tasks = null };
            projects.Add(y);
            projects.Add(x);

            VMTask result = tasks.FirstOrDefault(u => u.Id == id);

            if (result == null)
                return HttpNotFound();

            return View(result);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                // TODO: Update timestamp to use the one in model and not the one made up here...

                if (ModelState.IsValid)
                {
                    VMTask task = new VMTask();
                    task.Title = Request.Form["Title"];
                    task.Description = Request.Form["Description"];
                    task.CreatedDate = DateTime.UtcNow;

                    // TODO: Change from container to DB
                    //container.AddTask(task);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/5
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
