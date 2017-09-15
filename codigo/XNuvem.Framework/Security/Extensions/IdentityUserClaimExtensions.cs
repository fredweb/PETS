using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Security
{
    public static class IdentityUserClaimExtensions
    {
        public static Claim GetClaim(this IdentityUserClaim theThis) {
            return new Claim(theThis.ClaimType, theThis.ClaimValue);
        }
    }
}