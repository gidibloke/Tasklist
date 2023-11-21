using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webapp.Dtos
{
    public class UsersDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string RoleName { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsEnabled { get; set; }
    }
}