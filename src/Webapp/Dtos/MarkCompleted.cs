using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class MarkCompleted
    {
        public Guid TaskId { get; set; }

        public DateTime? CompletedDate { get; set; }

    }
}