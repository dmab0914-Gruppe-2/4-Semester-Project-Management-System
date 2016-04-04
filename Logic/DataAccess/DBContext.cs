using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Logic.Models;

namespace Logic.DataAccess
{
#pragma warning disable 0169    // disable never used warnings for fields that are being used by LINQ
    internal sealed class DbContext : DataContext
    {
        private const string ServerAdress = "localhost"; //Host adress. example localhost or ip or domain name.
        private const string ServerSubUrlName = "SQLEXPRESS"; //Fill if rdbms is located by url name. Example MS Sql server is usually "SQLEXPRESS". Else let it be empty or null.
        private const string Database = "PMS"; //The database name.
        private const string Username = "sa"; //DB username if not using integrated security.
        private const string Password = ""; //DB password if not using integrated security.
        private const bool UseIntegratedSecurity = true; //use integrated security instead of username and password.

        private static DbContext _instance = null;
        private static readonly object Threadlock = new Object();
        public static string Error { get; private set; }
        private DbContext()
            : base(ConnectionString())
        //: base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ProjectShare;Integrated Security=True")
        //: base("Data Source=kraka.ucn.dk;Initial Catalog=dmab0914_2Sem_2;User ID=dmab0914_2Sem_2;Password=IsAllowed")
        {
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private static string ConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder();
            if (!string.IsNullOrEmpty(ServerSubUrlName))
            {
                sqlConnectionString.DataSource = ServerAdress + @"\" + ServerSubUrlName;
            }
            else
            {
                sqlConnectionString.DataSource = ServerAdress;
            }
            sqlConnectionString.InitialCatalog = Database;
            sqlConnectionString.MultipleActiveResultSets = true;
            sqlConnectionString.ConnectTimeout = 10;
            if (UseIntegratedSecurity)
            {
                sqlConnectionString.IntegratedSecurity = true;
            }
            else
            {
                sqlConnectionString.UserID = Username;
                sqlConnectionString.Password = Password;
            }
            return sqlConnectionString.ToString();
        }

        public static DbContext Instance
        {
            get
            {
                lock (Threadlock)
                {
                    if (_instance != null) return _instance;
                    #region Bug
                    const string bug = "\nA bug have been found entering the application.." + 
                        @"
                                _               _
                               (o\             /o)
                               /::\  o     o  /::\
                              /:'':\  \___/  /:'':\
                             /:'  ':\/6 . 6\/:'  ':\
                            (o:.   '(  ._.  )'   .:o)      _.-'''-.
                             `\:.    \     /    .:/`     .'        '.
                               `\:.  /`---'\  .:/`       :           :
                                 `)://`===`\\:(`          '.        .'
                                 /:(/\-===-/\):\            '.    .'
                                /:'.,/ /^\ \,.':\             '..'
                               /:.:/(_/   \_)\:.:\            .''._
                              (::/`     :     `\::)         .'     `-....-'`
                               \o)       '.    (o/        .'
                                ^          '.   ^       .'
                                             `'''''''''`
                        ";
                    #endregion
                    try
                    {
                        _instance = new DbContext();
                        //(instance.Connection.State & ConnectionState.Broken) != 0
                        //Console.WriteLine("Connection string = " + instance.Connection.ConnectionString);
                        //Console.WriteLine("Connection state = " + instance.DatabaseExists());
                        if (!_instance.DatabaseExists())
                        {
                            //Connection to database failed. Time to be a bitch.
                            Error = "Can't access the database. The Connection string is:\n" + _instance.Connection.ConnectionString + bug;
                            Console.WriteLine(Error);
                            return null;
                        }
                    }
                    catch (SqlException e)
                    {
                        Error = "Something is wrong with the conneciton string or sql command: \n " + e;
                        return null;
                    }
                    catch (Exception e)
                    {
                        Error = "Something unexspected happened.. \n " + e;
                        return null;
                    }
                    return _instance;
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
