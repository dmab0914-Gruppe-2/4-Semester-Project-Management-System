using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DbProject
    {

        public static Project GetProject(int projectId)
        {
            DbContext dbContext = DbContext.Instance;
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

        public static List<Project> GetAllProjects()
        {
            DbContext dbContext = DbContext.Instance;
            return dbContext.Projects.ToList();
        }
    }
}
