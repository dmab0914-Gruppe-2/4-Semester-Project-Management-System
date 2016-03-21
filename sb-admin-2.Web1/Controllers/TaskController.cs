using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sb_admin_2.Web1.Models;
using TaskStatus = sb_admin_2.Web1.Models.TaskStatus;

namespace sb_admin_2.Web1.Controllers
{
    public class TaskController : Controller
    {
        private List<Models.Task> Tasks = new List<Models.Task>();
        private int y;
        // GET: Task
        public ActionResult Index()
        {
            Models.Task task = new Models.Task
            {
                Id = y++,
                Description = "Do the following........",
                Timestamp = DateTime.UtcNow,
                Status = TaskStatus.Assigned,
                Title = "Alright people, wake up!"
            };
            Tasks.Add(task);
            return View(Tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {

            Task taskk = new Models.Task
            {
                Id = y++,
                Description = "Do the following........",
                Timestamp = DateTime.UtcNow,
                Status = TaskStatus.Assigned,
                Title = "Alright people, wake up!"
            };
            Tasks.Add(taskk);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = Tasks.FirstOrDefault(x => x.Id == id);
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
                // TODO: Add insert logic here

                return RedirectToAction("Index");
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
