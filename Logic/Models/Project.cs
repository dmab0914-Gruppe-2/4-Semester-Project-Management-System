﻿using System;
using System.Collections.Generic;
//using Web_UI.Models;

namespace Logic.Models
{
    public class Project
    {
        //autogenerated id from db
        public int? Id { get; set; }

        //name of the project
        public string Title { get; set; }

        //description of the project
        public string Description { get; set; }
        //Selected admin of the project
        public User LeaderUser { get; set; }
        //list of assigned users to the project
        public List<User> Members { get; set; }
        //list of assigned tasks to the project
        public List<Task> Tasks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastChangeDateTime { get; set; }

    }
}