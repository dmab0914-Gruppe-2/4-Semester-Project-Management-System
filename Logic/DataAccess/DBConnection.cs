using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataAccess
{
    class DBConnection : IDisposable
    {
        private string serverUrl = "localhost";
        private string database = "PMS";
        private string username = "sa";
        private string password = "";
        private bool UseIntegratedSecurity = true;

        private static SqlConnection con;
        private static DBConnection instance = null;
        //Integrated Security = true

        private DBConnection()
        {
            string connectionString;
            if (UseIntegratedSecurity)
            {
                connectionString = "server=" + serverUrl + ";" +
                                   "database=" + database + ";" +
                                   //"Trusted_Connection=yes;" +
                                   "Integrated Security = true;" +
                                   "connection timeout=30";
            }
            else
            {
                connectionString = "user id=" + username + ";" +
                                   "password=" + password + ";" +
                                   "server=" + serverUrl + ";" +
                                   "database=" + database + "; " +
                                   "Trusted_Connection=yes;" +
                                   "connection timeout=30";
            }
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static DBConnection Singleton()
        {
            if (instance == null)
            {
                instance = new DBConnection();
            }
            return instance;
        }

        private System.Data.SqlClient.SqlCommand SqlCommand(string sqlString)
        {
            return new System.Data.SqlClient.SqlCommand(sqlString, con);
        }

        public SqlDataReader ReadData(string sqlString)
        {
            SqlDataReader myDataReader;
            try
            {
                myDataReader = SqlCommand(sqlString).ExecuteReader();
                return myDataReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        public void CLoseConnection()
        {
            try
            {
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Dispose()
        {
            CLoseConnection();
            GC.SuppressFinalize(this);
        }
    }
}
