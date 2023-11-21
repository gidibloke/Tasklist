using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class DashboardDto
    {
        public int AssociatedTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InprogressTasks { get; set; }
        public int NotStarted { get; set; }

    }
}
