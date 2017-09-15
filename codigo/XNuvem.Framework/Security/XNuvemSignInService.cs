using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XNuvem.Logging;

namespace XNuvem.Security
{
    public class XNuvemSignInService : SignInManager<User, string>, ISignInService
    {
        private readonly IUserService _userService;
        private readonly IOwinContext _owinContext;

        public ILogger Logger { get; set; }
        public XNuvemSignInService(IUserService userService, IOwinContext owinContext) :
            base(userService.UserManager, owinContext.Authentication) {
                _userService = userService;
                _owinContext = owinContext;
                Logger = NullLogger.Instance;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user) {
            return _userService.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        #region ISignInService...
        SignInManager<User, string> ISignInService.SignInManager {
            get { return this; }
        }

        async Task<SignInStatus> ISignInService.PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout) {
            var result =  await this.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            if (result != SignInStatus.Success) {
                Logger.Warning("Falha ao tentar efetuar o login. Usuário: {0}", userName);
            }
            return result;
        }

        void ISignInService.SignOut() {
            _owinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        #endregion

    }
    
}
