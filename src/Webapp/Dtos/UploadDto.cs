using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class UploadDto
    {
        [Required]
        public HttpPostedFileBase Picture { get; set; }
    }
}
