using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Webapp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFullName(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetJobTitle(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("JobTitle");
            return (claim != null) ? claim.Value : string.Empty;
        }
        
        public static string ProfilePicture(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("ProfilePicture");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}