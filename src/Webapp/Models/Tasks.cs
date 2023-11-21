using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Webapp.LookupModels;

namespace Webapp.Models
{
    public class Tasks
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string AssigningUserId { get; set; }
        [ForeignKey("AssigningUserId")]
        public ApplicationUser AssigningUser { get; set; }
        public string AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public ApplicationUser AssignedToUser { get; set; }
        public DateTime? DateAssigned { get; set; }
        public int? TaskStatusId { get; set; }
        public TasksStatus TaskStatus { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int PriorityId { get; set; }
        public Priority Priority { get; set; }


        public DateTime DateDue { get; set; }
    }
}