using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace allpax_service_record.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var name = principal.Claims.FirstOrDefault(c => c.Type == "name");
            return name?.Value;
        }
    }
}