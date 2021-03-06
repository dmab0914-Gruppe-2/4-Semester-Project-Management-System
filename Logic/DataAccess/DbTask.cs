﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbTask
    {
        private DbContext DbContext { get; set; }

        public DbTask()
        {
            DbContext = DbContext.Instance;
            if (DbContext == null)
            {
                //TODO Some sort of DBContext DbProject error handling?
                //throw new SqlConnectionException("Database connection failed!");
            }
        }
        public bool AddTask(Task task)
        {
            if (DbContext == null) return false;
            try
            {
                DbContext.Tasks.InsertOnSubmit(task);
                DbContext.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't add task to database. Task title: " + task.Title + "\nException is:\n" + e);
                return false;
            }
        }
        public Task GetTask(int taskId)
        {
            if (DbContext == null) return null;
            try
            {
                Task task = DbContext.Tasks.FirstOrDefault(i => i.Id == taskId);
                if (task != null)
                {
                    return task;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong, when looking for the given task id: " + taskId + " Error Message: \n" + e);
            }
            return null;
        }
        public List<Task> GetTask(string title)
        {
            if (DbContext == null) return null;
            var tasks = from task in DbContext.Tasks
                        where task.Title.Equals(title)
                        select task;
            return tasks.ToList();
        }

        public List<Task> GetTasksByProject(int projectId)
        {
            if (DbContext == null) return null;
            var dbTasks = from task in DbContext.Tasks
                        where task.ProjectId.Equals(projectId)
                        select task;
            List<Task> tasks = dbTasks.ToList();
            //if (tasks.Count == 0) return null;
            //for (int i = 0; i < tasks.Count; i++) //Rebuilds all tasks with leftover information from other places in database.
            //{
            //    Debug.Assert(tasks[i].Id != null, "tasks[i].Id != null");
            //    tasks[i] = GetTask(tasks[i].Id.Value);
            //}
            return tasks;
        }

        public List<Task> GetAllTasks()
        {
            if (DbContext == null) return null;
            return DbContext.Tasks.ToList();
        }

        public bool UpdateTasK(Task task)
        {
            if (DbContext == null) return false;
            bool error = false;
            try
            {
                var option = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    Debug.Assert(task.Id != null, "task.Id != null");
                    Task taskNew = GetTask(task.Id.Value);
                    List<bool> success = new List<bool>();
                    if (taskNew != null)
                    {
                        taskNew.ProjectId = task.ProjectId;
                        taskNew.Title = task.Title;
                        taskNew.Description = task.Description;
                        taskNew.Priority = task.Priority;
                        taskNew.Status = task.Status;
                        taskNew.Created = task.Created;
                        taskNew.LastEdited = task.LastEdited;
                        taskNew.DueDate = task.DueDate;
                        DbContext.SubmitChanges();
                        scope.Complete();
                    }
                    else
                    {
                        scope.Dispose();
                        error = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Task could not be updated. Task id: " + task.Id + " Error: \n" + e);
                error = true;
            }
            if (error != true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a given task.
        /// </summary>
        /// <param name="taskId">The id of the task to remove</param>
        /// <returns>True if sucessfull. False if not.</returns>
        public bool RemoveTask(int taskId)
        {
            if (DbContext == null) return false;
            Task task = GetTask(taskId);
            if (task != null) //In case the given task actually doesn't exist.. Then there's no reason to run thru the removal procedure.
            {
                bool error = false;
                try
                {
                    var option = new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted
                    };

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, option))
                    {
                        task = GetTask(taskId); //The task can have changed between last request and start of transactionscope.
                        List<bool> success = new List<bool>();
                        success.Add(true);
                        //Remove users here.
                        DbContext.Tasks.DeleteOnSubmit(task);
                        if (success.TrueForAll(x => x.Equals(true))) //Checks if all values in the List matches true, and returns true if so.
                        {
                            DbContext.SubmitChanges();
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                            error = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task could not be removed. task id: " + taskId + " Error: \n" + e);
                    error = true;
                }
                if (error != true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
