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
using Microsoft.AspNet.SignalR;
using Web_UI.Hubs;

namespace Web_UI.Controllers
{
    public class TaskController : Controller
    {
        private ITaskController TC = new Logic.Controllers.TaskController();
       

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
                    string dueDateDate = Request.Form["DueDateDate"];
                    string dueDateTime = Request.Form["DueDateTime"];
                    DateTime? dueDate = null;
                    if (dueDateDate != "" && dueDateTime != "")
                    {
                        dueDate = Utility.ParseDateTime(dueDateDate, dueDateTime);
                    }
                    int id = Convert.ToInt32(RouteData.Values["projectId"] + Request.Url.Query.Split('=')[1]);
                    if (title != null && title.Length > 0)
                    {
                        var theirStatus = MyEnumExtensions.ToLogicEnumStatus(status);
                        var theirPriority = MyEnumExtensions.ToLogicEnumPriority(priority);
                        ReturnValue result = TC.CreateTask(title, description, theirPriority, theirStatus, id, dueDate);

                        if (result == ReturnValue.Success)
                        {
                            var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                            context.Clients.All.changedTask();
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
            VMTask vt = new VMTask
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = TheirEnumExtensions.ToWebEnumTaskStatus(t.Status),
                Priority = TheirEnumExtensions.ToWebEnumPriority(t.Priority),
                CreatedDate = t.Created,                
                LastChangedDate = t.LastEdited,
                ProjectId = t.ProjectId
            };
            if(t.DueDate != null)
            {
                vt.DueDateDate = Utility.SplitDateTime(t.DueDate.Value)[0];
                vt.DueDateTime = Utility.SplitDateTime(t.DueDate.Value)[1];
            }
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
                    task.Description = Request.Form["description"];
                    task.Status = (Status)Enum.Parse(typeof(Status), Request.Form["Status"]);
                    task.Priority = (Priority)Enum.Parse(typeof(Priority), Request.Form["Priority"]);
                    task.DueDateDate = Request.Form["DueDateDate"];
                    task.DueDateTime = Request.Form["DueDateTime"];
                    DateTime? dueDate = null;
                    if (task.DueDateDate != "" && task.DueDateTime != "")
                    {
                        dueDate = Utility.ParseDateTime(task.DueDateDate, task.DueDateTime);
                    }
                    task.LastChangedDate = DateTime.UtcNow;
                    task.CreatedDate = DateTime.ParseExact(Request.Form["CreatedDate"], "MM-dd-yyyy H:mm:ss", null);
                    int projectid = TC.GetTask(id).ProjectId;


                    Task update = new Task
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = MyEnumExtensions.ToLogicEnumStatus(task.Status),
                        Priority = MyEnumExtensions.ToLogicEnumPriority(task.Priority),
                        DueDate = dueDate,
                        LastEdited = task.LastChangedDate,
                        Created = task.CreatedDate,
                        ProjectId = projectid
                    };

                    ReturnValue result = TC.UpdateTask(update);
                    if (result == ReturnValue.Success)
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                        context.Clients.All.changedTask();
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
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<ProjectHub>();
                        context.Clients.All.changedTask();
                        return RedirectToAction("Index", "Project");
                    }
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
