using System;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Logic.Models;

namespace Logic.DataAccess
{
#pragma warning disable 0169    // disable never used warnings for fields that are being used by LINQ
    sealed class DBContext : DataContext
    {
        private static readonly string serverAdress = "localhost"; //Host adress. example localhost or ip or domain name.
        private static readonly string serverSubUrlName = "SQLEXPRESS"; //Fill if rdbms is located by url name. Example MS Sql server is usually "SQLEXPRESS". Else let it be empty or null.
        private static readonly string database = "PMS"; //The database name.
        private static readonly string username = "sa"; //DB username if not using integrated security.
        private static readonly string password = ""; //DB password if not using integrated security.
        private static readonly bool UseIntegratedSecurity = true; //use integrated security instead of username and password.

        private static DBContext instance = null;
        private static readonly object threadlock = new Object();
        private DBContext()
            : base(ConnectionString())
        //: base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ProjectShare;Integrated Security=True")
        //: base("Data Source=kraka.ucn.dk;Initial Catalog=dmab0914_2Sem_2;User ID=dmab0914_2Sem_2;Password=IsAllowed")
        {
        }

        private static string ConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder();
            if (string.IsNullOrEmpty(serverSubUrlName))
            {
                sqlConnectionString.DataSource = serverAdress + @"\" + serverSubUrlName;
            }
            else
            {
                sqlConnectionString.DataSource = serverAdress;
            }
            sqlConnectionString.InitialCatalog = database;
            sqlConnectionString.MultipleActiveResultSets = true;
            if (UseIntegratedSecurity)
            {
                sqlConnectionString.IntegratedSecurity = true;
            }
            else
            {
                sqlConnectionString.UserID = username;
                sqlConnectionString.Password = password;
            }
            return sqlConnectionString.ToString();
        }

        public static DBContext Instance
        {
            get
            {
                lock (threadlock)
                {
                    if (instance != null) return instance;
                    try
                    {
                        instance = new DBContext();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Something is wrong with the conneciton string or sql command: \n " + e);
                        return null;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something unexspected happened.. \n " + e);
                        return null;
                    }
                    return instance;
                }
            }
        }


        //public Table<User> Users;
        public Table<Project> Projects;
        //public Table<ProjectUsers> ProjectUsers;
        //public Table<ProjectFiles> ProjectFiles;
        //public Table<ChatMessage> ChatMessages;
        //public Table<FileChat> FileChats;
    }
}
