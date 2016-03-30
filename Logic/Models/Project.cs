﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web1.Models
{
    public class Project
    {
        //autogenerated id from db
        public int? Id { get; set; }

        //name of the project
        public string Name { get; set; }

        //description of the project
        public string Description { get; set; }
        //Selected admin of the project
        public User LeaderUser { get; set; }
        //list of assigned users to the project
        public List<User> Members { get; set; }
        //list of assigned tasks to the project
        public List<Task> Tasks { get; set; }

    }
}