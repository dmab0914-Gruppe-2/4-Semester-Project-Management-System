﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;

//using Web_UI.Models;

namespace Logic.Models
{
    [Table]
    public class Project
    {
        //autogenerated id from db
        [Column(Name = "Id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        //name of the project
        [Column]
        public string Title { get; set; }

        //description of the project
        [Column]
        public string Description { get; set; }
        
        //project status
        [Column]
        public bool Done { get; set; }
        //We're using the ISO 8601 Standard for DateTime. 
        //YYYY-MM-DD hh:mm:ss.mss
        //2016-05-25 22:15:55.000
        [Column]
        public DateTime? CreatedDate { get; set; }
        [Column]
        public DateTime? LastChange { get; set; }
        //Selected admin of the project
        public User LeaderUser { get; set; }
        //list of assigned users to the project
        public List<User> Members { get; set; }
        //list of assigned tasks to the project
        public List<Task> Tasks { get; set; }
        

    }
}