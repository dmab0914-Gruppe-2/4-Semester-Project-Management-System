using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbTask
    {
        private DbContext DbContext {get; set;}

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
            //TODO
            return false;
        }
        public Logic.Models.Task GetTask(int taskId)
        {
            if (DbContext == null)
            {
                return null;
            }
            try
            {
                Logic.Models.Task task = DbContext.Tasks.FirstOrDefault(i => i.Id == taskId);
                if (task != null)
                {
                    return task;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong, when looking for the given task id: " + taskId + " Error Message: \n" + e);
            }
            return null;
        }
        public List<Logic.Models.Task> GetTask(string title)
        {
            if (DbContext == null)
            {
                return null;
            }
            var tasks = from task in DbContext.Tasks
                        where task.Title.Equals(title)
                        select task;
            List<Logic.Models.Task> taskList = tasks.ToList();

            return taskList;
        }
        public List<Logic.Models.Task> GetAllTasks()
        {
            if (DbContext == null)
            {
                return null;
            }
            return DbContext.Tasks.ToList();
        }
    }
}
