using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class TaskIndexDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string AssignedTo { get; set; }
        public int? Status { get; set; }
        public int? Priority { get; set; }
        public bool CompletedYN { get; set; }
        public DateTime? DateDue { get; set; }



    }
}