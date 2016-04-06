using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Logic.Models;
#pragma warning disable 162     //Disable unreachable code warning.
#pragma warning disable 0169    //Disable never used warnings for fields that are being used by LINQ

namespace Logic.DataAccess
{
    public sealed class DbContext : DataContext
    {
        private const string ServerAdress = "localhost"; //Host adress. example localhost or ip or domain name.
        private const string ServerSubUrlName = "SQLEXPRESS"; //Fill if rdbms is located by url name. Example MS Sql server is usually "SQLEXPRESS". Else let it be empty or null.
        private const string Database = "PMS"; //The database name.
        private const string Username = "sa"; //DB username if not using integrated security.
        private const string Password = ""; //DB password if not using integrated security.
        private const bool UseIntegratedSecurity = true; //use integrated security instead of username and password.

        private static DbContext _instance = null;
        private static readonly object Threadlock = new object();
        public static string Error { get; private set; }
        private DbContext()
            : base(ConnectionString())
        //: base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ProjectShare;Integrated Security=True")
        //: base("Data Source=kraka.ucn.dk;Initial Catalog=dmab0914_2Sem_2;User ID=dmab0914_2Sem_2;Password=IsAllowed")
        {
        }

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
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

        /// <summary>
        /// Gets the DbContext instance or creates it if it doesn't exist.
        /// </summary>
        /// <returns>
        /// Returns the instance of DbContext if Database connection is sucessfull. Null if not.
        /// </returns>
        /// <remarks>
        /// If there is something wrong with the database connection, it will save the error to the static field "Error", print it to console, and return null.
        /// </remarks>
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
                    _instance = new DbContext();
                    if (IsDatabaseConnected()) return _instance;
                    Console.WriteLine(Error);
                    Console.WriteLine(bug);
                    return _instance;
                }
            }
        }

        public static bool IsDatabaseConnected()
        {
            if (_instance.DatabaseExists()) return true;
            //Connection to database failed. The error can be retrived from the public static field Error.
            Error = "Can't access the database. The Connection string is:\n" + _instance.Connection.ConnectionString;
            _instance = null;
            return false;
        }


        //public Table<User> Users;
        public Table<Project> Projects;
        //public Table<ProjectUsers> ProjectUsers;
        //public Table<ProjectFiles> ProjectFiles;
        //public Table<ChatMessage> ChatMessages;
        //public Table<FileChat> FileChats;
    }
}
