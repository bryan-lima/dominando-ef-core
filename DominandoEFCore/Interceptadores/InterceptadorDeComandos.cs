using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadorDeComandos : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine("[Sync] Entrei dentro do método ReaderExecuting");
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("[Async] Entrei dentro do método ReaderExecuting");
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
