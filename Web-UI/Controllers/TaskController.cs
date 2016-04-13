using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Logic.Models;
using Logic.Controllers;
using Web_UI.Models;
using Logic;
using Web_UI.Models.Enums;
using Priority = Web_UI.Models.Enums.Priority;

namespace Web_UI.Controllers
{
    public class TaskController : Controller
    {
        private ITaskController TC = new Logic.Controllers.TaskController();
        // GET: Task
        public ActionResult Index()
        {

            return View();
        }

        // GET: Task/Create
        public ActionResult Create(int projectId)
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
                    Status status = (Status)Enum.Parse(typeof(Status), Request.Form["Status"]);
                    Priority priority = (Priority)Enum.Parse(typeof(Priority), Request.Form["Priority"]);
                    DateTime dueDate = DateTime.ParseExact(Request.Form["DueDate"], "yyyy-MM-dd", null);
                    int id = Convert.ToInt32(RouteData.Values["projectId"] + Request.Url.Query.Split('=')[1]);
                    if (title != null && title.Length > 0)
                    {
                        var theirStatus = MyEnumExtensions.ToLogicEnumStatus(status);
                        var theirPriority = MyEnumExtensions.ToLogicEnumPriority(priority);
                        ReturnValue result = TC.CreateTask(title, description, theirPriority, theirStatus, id, dueDate);

                        if (result == ReturnValue.Success)
                        {
                            return RedirectToAction("Index", "Project");
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            Task t = TC.GetTask(id);

            VMTask vt = new VMTask(t.Id, t.Title, t.Description, TheirEnumExtensions.ToWebEnumTaskStatus(t.Status), TheirEnumExtensions.ToWebEnumPriority(t.Priority), t.Created, t.DueDate, t.LastEdited, t.ProjectId);
            if(t == null)
            {
                return HttpNotFound();
            }

            return View(vt);
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
