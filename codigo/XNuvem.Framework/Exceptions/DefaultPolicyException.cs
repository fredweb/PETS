using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.Data;
using XNuvem.Logging;
using XNuvem.UI.Messages;

namespace XNuvem.Exceptions
{
    public class DefaultPolicyException : IPolicyException
    {
        ITransactionManager _transactionManager;

        public ILogger Logger { get; set; }

        public DefaultPolicyException(ITransactionManager transactionManager) {
            _transactionManager = transactionManager;
            Logger = NullLogger.Instance;
        }

        public bool HandleException(Exception ex) {
            try {
                Logger.Error(ex, "Erro inesperado ao executar uma operação");
                Logger.Warning("Canceling transaction due to an error");
                _transactionManager.Cancel();
                if (ex.IsFatal()) {
                    return false;
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
