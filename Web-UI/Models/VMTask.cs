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
        public VMProject Project { get; set; }
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
        [DataType(DataType.Date)]
        [DisplayName("Created on")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Due date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Last changed")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime LastChangedDate { get; set; }
    }
}