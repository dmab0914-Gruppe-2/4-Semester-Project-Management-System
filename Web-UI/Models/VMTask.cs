using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_UI.Models
{
    public class VMTask
    {
        //id from db
        public int? Id { get; set; }
        // Project
        public int ProjectId { get; set; }
        //title of task
        public string Title { get; set; }
        //description of task
        public string Description { get; set; }
        //status of task
        public Enums.Status Status { get; set; }
        //Priority of task
        public Enums.Priority Priority { get; set; }
        //assigned user for the task
        //TODO Can be changed to array or list later for convience
        public VMUser AssignedUser { get; set; }
        //Timestamp of change / creation. TBD


        [DisplayName("Created on")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
       
        
        [DisplayName("Due date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public string DueDateDate { get; set; }
        public string DueDateTime { get; set; }


        [DisplayName("Last changed")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? LastChangedDate { get; set; }
       
        
        public VMTask()
        {

        }
        public VMTask(int? id, string title, string desc, Enums.Status status, Enums.Priority priority, DateTime created, DateTime? duedate, DateTime? lastedited, int projectId)
        {
            this.Id = id;
            this.Title = title;
            this.Description = desc;
            this.Status = status;
            this.Priority = priority;
            this.CreatedDate = created;
            this.DueDate = duedate;
            this.LastChangedDate = lastedited;
            this.ProjectId = projectId;
        }

    }
}