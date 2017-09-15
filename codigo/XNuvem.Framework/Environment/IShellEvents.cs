using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Environment
{
    public interface IShellEvents
    {
        /// <summary>
        /// Fired when initialize the shell
        /// </summary>
        void OnInitialize();

        /// <summary>
        /// Fired when terminate the shell
        /// </summary>
        void OnTerminate();

        /// <summary>
        /// Fired when begin the request
        /// </summary>
        /// <param name="context"><see cref="IOwinContext"/></param>
        void OnBeginRequest(IOwinContext context);

        /// <summary>
        /// Fired when request is ended
        /// </summary>
        /// <param name="context"><see cref="IOwinContext"/></param>
        void OnEndRequest(IOwinContext context);
    }
}
