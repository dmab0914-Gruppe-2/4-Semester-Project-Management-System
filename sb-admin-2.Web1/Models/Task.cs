using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace sb_admin_2.Web1.Models
{
    public class Task
    {
        //id from db
        public int? Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        //title of task
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        //description of task
        [StringLength(100)]
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