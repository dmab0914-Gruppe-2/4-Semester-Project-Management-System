using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.DataAccess
{
    public class DBProject
    {
        public static Project GetProject(int projectID)
        {
            string sql = @"select * from Project where id = "
                            + projectID;

            //dbCmd = AccessDbSQLClient.GetDbCommand(sql);

            //IDataReader dbReader;
            //dbReader = dbCmd.ExecuteReader();

            IDataReader dbReader = DBConnection.Singleton().ReadData(sql);

            Project project;

            if (dbReader.Read())
            {
                project = new Project();
                project.Id = Convert.ToInt32(dbReader["id"].ToString());
                project.Name = dbReader["name"].ToString();
                project.Description = dbReader["description"].ToString();
                //Convert.ToInt32(dbReader["custNo"].ToString()),dbReader["name"].ToString());
            }
            else
            {
                project = null;
            }
            dbReader.Close();
            DBConnection.Singleton().CLoseConnection();
            return project;
        }
    }
}
