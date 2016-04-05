using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbProject
    {
        public bool AddProject(Project project)
        {
            DbContext dbContext = DbContext.Instance;
            if (dbContext == null)
            {
                return false;
            }
            try
            {
                dbContext.Projects.InsertOnSubmit(project);
                dbContext.SubmitChanges();
                if (true) //TODO check if the data added to db were sucessfull / valid.
                    return true;
            }
            catch (Exception e)
            {
                throw new Exception("Project not added to db " + e);
            }
        }

        public static Project GetProject(int projectId)
        {
            DbContext dbContext = DbContext.Instance;
            if (dbContext == null)
            {
                return null;
            }
            try
            {
                Project project = dbContext.Projects.FirstOrDefault(i => i.Id == projectId);
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
            DbContext dbContext = DbContext.Instance;
            if (dbContext == null)
            {
                return null;
            }
            var projects = from project in dbContext.Projects
                           where project.Title.Equals(title)
                           select project;
            List<Project> projectsList = projects.ToList();
            //for (int i = 0; i < projectsList.Count; i++)
            //{
            //    projectsList[i] = GetProject(projectsList[i].Id);
            //}
            return projectsList;
        }

        public static List<Project> GetAllProjects()
        {
            DbContext dbContext = DbContext.Instance;
            if (dbContext == null)
            {
                return null;
            }
            return dbContext.Projects.ToList();
        }
    }
}
