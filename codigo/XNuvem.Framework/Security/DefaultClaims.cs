using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Security
{
    public static class DefaultClaims
    {
        public static Claim Administrator = new Claim("Role", "Administrator");

        public static Claim SiteUser = new Claim("Role", "SiteUser");
    }
}
