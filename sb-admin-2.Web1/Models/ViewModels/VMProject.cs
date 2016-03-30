using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web1.Models.ViewModels
{
    /// <summary>
    /// Samler alt data der skal vises på skærmen
    /// </summary>
    public class VMProject
    {
        public string Projectname { get; set; }
        public string Description { get; set; }
        public List<Task> Tasks { get; set; }
    }
}