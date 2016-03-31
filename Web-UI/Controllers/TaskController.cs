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
                Timestamp = DateTime.UtcNow,
                //Status = TaskStatus.Assigned,
                Title = "Alright people, wake up!"
            };
            Tasks.Add(task);
            return View(Tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {

            VMTask taskk = new VMTask
            {
                Id = y++,
                Description = "Do the following........",
                Timestamp = DateTime.UtcNow,
                //Status = TaskStatus.Assigned,
                Title = "Alright people, wake up!"
            };
            Tasks.Add(taskk);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMTask task = Tasks.FirstOrDefault(x => x.Id == id);
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
                            Timestamp = DateTime.UtcNow,

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
            return View();
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
                    task.Timestamp = DateTime.UtcNow;

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
