/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 *
 * 
/****************************************************************************************/


using NHibernate.AspNet.Identity;
using XNuvem.Data;

namespace XNuvem.Security
{
    public class IdentityUserClaimMap : EntityMap<IdentityUserClaim>
    {
        public IdentityUserClaimMap() {
            Table("AspNetUserClaims");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.ClaimType).Length(255);

            Map(x => x.ClaimValue).Length(255);

            References<IdentityUser>(x => x.User);
        }
    }
    public class IdentityRoleMap : EntityMap<IdentityRole>
    {
        public IdentityRoleMap()
        {
            this.Table("AspNetRoles");
            
            this.Id(x => x.Id).Length(50).GeneratedBy.UuidHex("D");
            
            this.Map(x => x.Name).Length(255).Not.Nullable().Unique();

            this.HasManyToMany(x => x.Users)
                .Table("AspNetUserRoles")
                .Cascade.None()
                .Not.LazyLoad();
        }
    }

    public class IdentityUserMap : EntityMap<IdentityUser>
    {
        public IdentityUserMap() {
            this.Table("AspNetUsers");
            this.Id(x => x.Id).GeneratedBy.UuidHex("D").Length(50);

            DiscriminateSubClassesOnColumn("ClassType").Length(100);

            this.Map(x => x.AccessFailedCount);

            this.Map(x => x.Email).Length(255).Not.Nullable();

            this.Map(x => x.EmailConfirmed);

            this.Map(x => x.LockoutEnabled);

            this.Map(x => x.LockoutEndDateUtc);

            this.Map(x => x.PasswordHash).Length(550);

            this.Map(x => x.PhoneNumber).Length(100);

            this.Map(x => x.PhoneNumberConfirmed);

            this.Map(x => x.TwoFactorEnabled);

            this.Map(x => x.UserName).Length(100).Not.Nullable().Unique();

            this.Map(x => x.SecurityStamp).Length(50);

            this.HasMany(x => x.Claims)
                .Not.LazyLoad()
                .Cascade.AllDeleteOrphan();

            this.HasMany(x => x.Logins)
                .Not.LazyLoad()
                .Cascade.AllDeleteOrphan()
                .Component(comp => {
                    comp.Map(p => p.LoginProvider);
                    comp.Map(p => p.ProviderKey);
                });

            this.HasManyToMany(x => x.Roles)
                .Table("AspNetUserRoles")
                .Not.LazyLoad();
        }
    }
}
