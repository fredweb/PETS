﻿/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 *
 * 
/****************************************************************************************/

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using NHibernate;
using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using XNuvem.Data;
using XNuvem.Logging;
using XNuvem.Security.Permissions;
using Autofac;
using Autofac.Integration.Owin;

namespace XNuvem.Security
{
    public class XNuvemUserService : UserManager<User>, IUserService
    {
        private readonly ITransactionManager _transactionManager;

        public ISession Session { get { return _transactionManager.GetSession(); } }

        public ILogger Logger { get; set; }

        public XNuvemUserService(ITransactionManager transactionManager)
            : base (new UserStore<User>(transactionManager.GetSession()) { 
                ShouldDisposeSession = false })
        {
            _transactionManager = transactionManager;
            SetAccountConfiguration();
            Logger = NullLogger.Instance;
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType) {
            Logger.Debug("Creating identity for user {0}", user.UserName);
            return base.CreateIdentityAsync(user, authenticationType);
        }

        #region IUserService...

        UserManager<User> IUserService.UserManager {
            get { return this; }
        }

        void IUserService.Create(User user, string password) {
            this.Create(user, password);
        }

        void IUserService.Update(User user) {
            this.Update(user);
        }

        void IUserService.Delete(User user) {
            this.Delete(user);
        }        

        User IUserService.FindByName(string userName) {
            return this.FindByName(userName);
        }

        IQueryable<User> IUserService.Users {
            get {
                return this.Users;
            }
        }

        User IUserService.GetCurrentUser() {
            if (!HostingEnvironment.IsHosted || HttpContext.Current == null) {
                Logger.Debug("Não é possível acessar o contexto da execução web.");
                return null;
            }
            var identity = HttpContext.Current.User;
            if (identity == null || identity.Identity == null || string.IsNullOrEmpty(identity.Identity.Name)) {
                return null;
            }

            return this.FindByName(identity.Identity.Name);
        }

        void IUserService.AssignPermissions(User user, IEnumerable<Permission> permissions) {
            if (user.Claims != null) {
                var claimsToRemove = user.Claims.Where(c => c.ClaimType == "Permission").ToList();
                foreach (var claim in claimsToRemove) {
                    this.RemoveClaim(user.Id, claim.GetClaim());
                }
            }
            foreach (var perm in permissions) {
                this.AddClaim(user.Id, new Claim("Permission", perm.Name));
            }
        }

        async Task IUserService.ForceChangePasswordAsync(User user, string newPassword) {
            using (var userStore = new UserStore<User>(Session)) {
                userStore.ShouldDisposeSession = false;
                string passwordHash = this.PasswordHasher.HashPassword(newPassword);
                await userStore.SetPasswordHashAsync(user, passwordHash);
            }
        }

        #endregion

        public static UserManager<User> Create(IdentityFactoryOptions<UserManager<User>> options, IOwinContext context) {
            return  context
                        .GetAutofacLifetimeScope()
                        .Resolve<IUserService>()
                        .UserManager;
          }


        internal void SetAccountConfiguration() {
            // Configure logic for user names
            this.UserValidator = new UserValidator<User>(this) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for password
            this.PasswordValidator = new PasswordValidator() {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;            
        }
    }
}
