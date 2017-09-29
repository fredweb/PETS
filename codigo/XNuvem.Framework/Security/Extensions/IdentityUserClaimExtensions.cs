using NHibernate.AspNet.Identity;
using System.Security.Claims;

namespace XNuvem.Security
{
    public static class IdentityUserClaimExtensions
    {
        public static Claim GetClaim(this IdentityUserClaim theThis) {
            return new Claim(theThis.ClaimType, theThis.ClaimValue);
        }
    }
}