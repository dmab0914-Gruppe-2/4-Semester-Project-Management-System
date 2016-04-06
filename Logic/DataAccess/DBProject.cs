using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbProject
    {
        private DbContext DbContext { get; set; }
        public DbProject()
        {
            DbContext = DbContext.Instance;
            if (DbContext == null)
            {
                //throw new SqlConnectionException("Database connection failed!");
            }
        }

        public bool AddProject(Project project)
        {
            if (DbContext == null)
            {
                return false;
            }
            try
            {
                DbContext.Projects.InsertOnSubmit(project);
                DbContext.SubmitChanges();
                if (true) //TODO check if the data added to db were sucessfull / valid.
                    return true;
            }
            catch (Exception e)
            {
                throw new Exception("Project not added to db " + e);
            }
        }

        public Project GetProject(int projectId)
        {
            if (DbContext == null)
            {
                return null;
            }
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
            if (DbContext == null)
            {
                return null;
            }
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
            if (DbContext == null)
            {
                return null;
            }
            return DbContext.Projects.ToList();
        }
    }
}
