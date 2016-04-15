using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    DateTime dueDate = DateTime.ParseExact(Request.Form["DueDate"], "dd-MM-yyyy H:mm:ss", null);
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
            if (t == null)
            {
                return HttpNotFound();
            }
            //VMTask vt = new VMTask(t.Id, t.Title, t.Description, TheirEnumExtensions.ToWebEnumTaskStatus(t.Status), TheirEnumExtensions.ToWebEnumPriority(t.Priority), t.Created, t.DueDate, t.LastEdited, t.ProjectId);
            Debug.Assert(t.DueDate != null, "t.DueDate != null");
            VMTask vt = new VMTask
            {
                Id = t.Id,
                Description = t.Description,
                Status = TheirEnumExtensions.ToWebEnumTaskStatus(t.Status),
                Priority = TheirEnumExtensions.ToWebEnumPriority(t.Priority),
                CreatedDate = t.Created,
                DueDateDate = Utility.SplitDateTime(t.DueDate.Value)[0],
                DueDateTime = Utility.SplitDateTime(t.DueDate.Value)[1],
                LastChangedDate = t.LastEdited,
                ProjectId = t.ProjectId
            };
            return View(vt);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    VMTask task = new VMTask();
                    task.Id = id;
                    task.Title = Request.Form["Title"];
                    task.Description = Request.Form["Description"];
                    task.Status = (Status)Enum.Parse(typeof(Status), Request.Form["Status"]);
                    task.Priority = (Priority)Enum.Parse(typeof(Priority), Request.Form["Priority"]);
                    //task.DueDate = DateTime.ParseExact(Request.Form["DueDate"], "dd-MM-yyyy H:mm:ss", null); // value = 27-05-2016 11:9:43 from the request form
                    task.DueDateDate = Request.Form["DueDateDate"];
                    task.DueDateTime = Request.Form["DueDateTime"];
                    task.LastChangedDate = DateTime.UtcNow;
                    task.CreatedDate = DateTime.ParseExact(Request.Form["CreatedDate"], "dd-MM-yyyy H:mm:ss", null);
                    int projectid = TC.GetTask(id).ProjectId;


                    Task update = new Task
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = MyEnumExtensions.ToLogicEnumStatus(task.Status),
                        Priority = MyEnumExtensions.ToLogicEnumPriority(task.Priority),
                        //DueDate = task.DueDate,
                        DueDate = Utility.ParseDateTime(task.DueDateDate, task.DueDateTime),
                        LastEdited = task.LastChangedDate,
                        Created = task.CreatedDate,
                        ProjectId = projectid
                    };

                    ReturnValue result = TC.UpdateTask(update);
                    if (result == ReturnValue.Success)
                    {
                        return RedirectToAction("Index", "Project");
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

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            Task t = TC.GetTask(id);
            VMTask vt = new VMTask();
            vt.Id = t.Id;
            vt.Title = t.Title;
            vt.Description = t.Description;
            vt.Status = TheirEnumExtensions.ToWebEnumTaskStatus(t.Status);
            vt.Priority = TheirEnumExtensions.ToWebEnumPriority(t.Priority);
            vt.CreatedDate = t.Created;
            vt.DueDate = t.DueDate;
            vt.LastChangedDate = t.LastEdited;
            vt.ProjectId = t.ProjectId;

            return View(vt);
        }

        // POST: Task/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Task t = TC.GetTask(id);
                if (t.Id != null)
                {
                    ReturnValue result = TC.RemoveTask((int)t.Id);

                    if (result == ReturnValue.Success)
                        return RedirectToAction("Index", "Project");
                    else
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
