using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Security.Permissions
{
    public interface IPermissionProvider
    {
        IEnumerable<Permission> GetPermissions();
    }
}
