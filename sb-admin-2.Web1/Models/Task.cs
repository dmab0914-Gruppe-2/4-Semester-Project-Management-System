using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace sb_admin_2.Web1.Models
{
    public class Task
    {
        //id from db
        public int Id { get; set; }
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
        //Timestamp of change / creation. TBD
        public DateTime Timestamp { get; set; }
    }
}