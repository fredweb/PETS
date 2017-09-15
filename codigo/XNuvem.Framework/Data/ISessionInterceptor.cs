using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Data
{
    /// <summary>
    /// Describes an NHibernate session interceptor, instantiated per-session.
    /// </summary>
    public interface ISessionInterceptor : IInterceptor, IDependency {
    }
}
