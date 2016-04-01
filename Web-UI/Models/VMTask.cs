using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_UI.Models
{
    public class VMTask
    {
        //id from db
        public int? Id { get; set; }

        public VMProject Project { get; set; }

        //title of task
        public string Title { get; set; }

        //description of task
        public string Description { get; set; }

        //status of task
        //TODO Uncomment line below when ready...
        //public TaskStatus Status { get; set; }

        //assigned user for the task
        //TODO Can be changed to array or list later for convience
        public VMUser AssignedUser { get; set; }

        //Timestamp of change / creation. TBD
        public DateTime Timestamp { get; set; }
    }
}