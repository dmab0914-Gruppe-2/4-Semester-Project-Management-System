﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Logic.Models
{
    public class Task
    {
        //id from db
        public int? Id { get; set; }

        public int ProjectId { get; set; }

        //title of task
        public string Title { get; set; }

        //description of task
        public string Description { get; set; }

        //status of task
        public TaskStatus Status { get; set; }

        //assigned user for the task
        //TODO Can be changed to array or list later for convience
        public User AssignedUser { get; set; }

        //Timestamp of creation
        public DateTime CreateDateTime { get; set; }

        //Timestamp of last edit
        public DateTime LastEditedDateTime { get; set; }

        //Timestamp of duedate
        public DateTime DueDateTime { get; set; }

    }
}