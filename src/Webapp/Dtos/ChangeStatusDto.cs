using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class ChangeStatusDto
    {
        public Guid TaskId { get; set; }

        public int StatusId { get; set; }
    }
}