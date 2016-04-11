using System;
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
                //throw new SqlConnectionException("Database connection failed!");
            }
        }
        public bool AddTask()
        {
            throw new NotImplementedException();
            //TODO
            return false;
        }
        public Logic.Models.Task GetTask(int taskId)
        {
            if (DbContext == null) return null;
            try
            {
                Logic.Models.Task task = DbContext.Tasks.FirstOrDefault(i => i.Id == taskId);
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
        public List<Logic.Models.Task> GetTask(string title)
        {
            if (DbContext == null) return null;
            var tasks = from task in DbContext.Tasks
                        where task.Title.Equals(title)
                        select task;
            List<Logic.Models.Task> taskList = tasks.ToList();

            return taskList;
        }
        public List<Logic.Models.Task> GetAllTasks()
        {
            if (DbContext == null) return null;
            return DbContext.Tasks.ToList();
        }

        public bool UpdateTast(Task task) //TODO test
        {
            //throw new NotImplementedException();
            if (DbContext == null) return false;
            bool error = false;
            try
            {
                var option = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
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
                Console.WriteLine("Task could not be removed. Task id: " + task.Id + " Error: \n" + e);
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
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
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
