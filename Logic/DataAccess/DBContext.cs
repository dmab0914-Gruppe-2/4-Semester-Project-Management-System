using System;
using System.Data.Linq;
using System.Linq;
using Logic.Models;

namespace Logic.DataAccess
{
    sealed class DbContext : DataContext
    {
        private static DbContext instance = null;
        private static readonly object threadlock = new Object();
        private DbContext()
            : base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ProjectShare;Integrated Security=True")
            //: base("Data Source=kraka.ucn.dk;Initial Catalog=dmab0914_2Sem_2;User ID=dmab0914_2Sem_2;Password=IsAllowed")
        {

        }

        public static DbContext Instance
        {
            get
            {
                lock (threadlock)
                {
                    if (instance == null)
                    {
                        instance = new DbContext();
                    }
                    return instance;
                }
            }
        }


        public Table<User> Users;
        public Table<Project> Projects;
        //public Table<ProjectUsers> ProjectUsers;
        //public Table<ProjectFiles> ProjectFiles;
        //public Table<ChatMessage> ChatMessages;
        //public Table<FileChat> FileChats;
    }
}
