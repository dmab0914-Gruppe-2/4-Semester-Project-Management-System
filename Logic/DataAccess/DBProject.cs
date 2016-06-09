using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbProject
    {
        private DbContext DbContext { get; set; }
        private DbTask DbTask { get; set; }
        public DbProject()
        {
            DbContext = DbContext.Instance;
            if (DbContext == null)
            {
                //TODO Some sort of DBContext DbProject error handling?
                //throw new SqlConnectionException("Database connection failed!");
            }
            else
            {
                DbTask = new DbTask();
            }
        }

        public bool AddProject(Project project)
        {
            if (DbContext == null) return false;

            try
            {
                DbContext.Projects.InsertOnSubmit(project);
                DbContext.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't add project to database. Project title: " + project.Title + "\nException is:\n" + e);
                return false;
            }
        }

        public Project GetProject(int projectId)
        {
            if (DbContext == null) return null;
            try
            {
                Project project = DbContext.Projects.FirstOrDefault(i => i.Id == projectId);
                if (project != null)
                {
                    //project.ProjectMembers = GetProjectMembers(projectId);
                    //project.ProjectFiles = dbFile.GetAllFilesForProject(project.Id);
                    //project.ProjectAdministrators = GetProjectAdministrators(project.Id);
                    return project;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong, when looking for the given Project id: " + projectId + " Error Message: \n" + e);
            }
            return null;

            #region DBConnection related pure SQL

            //string sql = @"select * from Project where id = "
            //                + projectID;

            ////dbCmd = AccessDbSQLClient.GetDbCommand(sql);

            ////IDataReader dbReader;
            ////dbReader = dbCmd.ExecuteReader();

            //IDataReader dbReader = DBConnection.Singleton().ReadData(sql);

            //Project project;

            //if (dbReader.Read())
            //{
            //    project = new Project();
            //    project.Id = Convert.ToInt32(dbReader["id"].ToString());
            //    project.Name = dbReader["name"].ToString();
            //    project.Description = dbReader["description"].ToString();
            //    //Convert.ToInt32(dbReader["custNo"].ToString()),dbReader["name"].ToString());
            //}
            //else
            //{
            //    project = null;
            //}
            //dbReader.Close();
            //DBConnection.Singleton().CLoseConnection();
            //return project;

            #endregion
        }

        /// <summary>
        /// Get projects with given title.
        /// </summary>
        /// <param name="title">The title of othe project(s)</param>
        /// <returns>A list with the project(s), where the title matches</returns>
        public List<Project> GetProject(string title)
        {
            if (DbContext == null) return null;
            var projects = from project in DbContext.Projects
                           where project.Title.Equals(title)
                           select project;
            List<Project> projectsList = projects.ToList();
            //for (int i = 0; i < projectsList.Count; i++)
            //{
            //    projectsList[i] = GetProject(projectsList[i].Id);
            //}
            return projectsList;
        }

        public List<Project> GetAllProjects()
        {
            if (DbContext == null) return null;
            return DbContext.Projects.ToList();
        }

        public bool UpdateProject(Project project)
        {
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
                    Debug.Assert(project.Id != null, "project.Id != null");
                    Project projectNew = GetProject(project.Id.Value);
                    List<bool> success = new List<bool>();
                    if (projectNew != null)
                    {
                        projectNew.Title = project.Title;
                        projectNew.Description = project.Description;
                        projectNew.Done = project.Done;
                        projectNew.CreatedDate = project.CreatedDate;
                        projectNew.LastChange = project.LastChange;
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
                Console.WriteLine("Project could not be updated. Project id: " + project.Id + " Error: \n" + e);
                error = true;
            }
            if (error != true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a given project and all dependencies in the form of tasks.
        /// </summary>
        /// <param name="projectId">The id of the project to remove</param>
        /// <returns>True if sucessfull. False if not.</returns>
        public bool RemoveProject(int projectId)
        {
            if (DbContext == null) return false;
            Project project = GetProject(projectId);
            if (project != null) //Incase the given project actually doesn't exist.. Then there's no reason to run thru the removal procedure.
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
                        project = GetProject(projectId); //The project can have changed between last request and start of transactionscope.
                        List<bool> success = new List<bool>();
                        if (project.Tasks != null)
                        {
                            foreach (Task task in project.Tasks)
                            {
                                if (task.Id != null) success.Add(DbTask.RemoveTask(task.Id.Value));
                                else success.Add(false);
                            }
                        }
                        else
                        {
                            success.Add(true);
                        }
                        DbContext.Projects.DeleteOnSubmit(project);
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
                    Console.WriteLine("Project could not be removed. Project id: " + projectId + " Error: \n" + e);
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
