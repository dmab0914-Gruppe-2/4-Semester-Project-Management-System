using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Logic.Models
{
    [Table]
    public class Task
    {
        //id from db
        [Column(Name = "Id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int? Id { get; set; }
        [Column]
        public int ProjectId { get; set; }

        //title of task
        [Column]
        public string Title { get; set; }

        //description of task
        [Column]
        public string Description { get; set; }

        //Prority of task
        [Column]
        public Priority Priority { get; set; }

        //status of task
        [Column]
        public TaskStatus Status { get; set; }

        //assigned user for the task
        //TODO Can be changed to array or list later for convience
        public User AssignedUser { get; set; }

        //Timestamp of creation
        [Column]
        public DateTime Created { get; set; }

        //Timestamp of last edit
        [Column]
        public DateTime? LastEdited { get; set; }

        //Timestamp of duedate
        [Column]
        public DateTime? DueDate { get; set; }

    }
}