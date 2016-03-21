﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sb_admin_2.Web1.Models;

namespace sb_admin_2.Web1.Controllers
{
    
    

    public class ProjectController : Controller
    {
        //TEMP DATA ACCESS
        private List<Project> projects = new List<Project>();
        private int z;

        // GET: Project
        public ActionResult Index()
        {
            Project y = new Project { Description = "Awesome project", Id = z++, Name = "42" };
            projects.Add(y);
            return View(projects.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            Project y = new Project {Description = "Awesome project", Id = z++, Name = "42"};
            projects.Add(y);
            if (id == null)
            {
                //Console.WriteLine("Id is equal to null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //todo Change to impliment data access
            Project project = projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
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
                    //todo insert dbinsert here....
                    //projects.Add(collection);
                    string name = Request.Form["name"];
                    string description = Request.Form["description"];
                    if (name != null && description != null)
                    {
                        projects.Add(new Project
                        {
                            Name = name,
                            Description = description
                        });
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
            return View();
        }

        // POST: Project/Edit/5
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
