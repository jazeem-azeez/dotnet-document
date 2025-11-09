using System;
using System.CommandLine;
using System.Diagnostics;
using System.Threading.Tasks;
using DotnetDocument.Tools.Utils;

namespace DotnetDocument.Tools.CLI
{
    /// <summary>
    /// The measure middleware class
    /// </summary>
    internal static class MeasureMiddleware
    {
        /// <summary>
        /// Handles the context
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="next">The next</param>
        internal static async Task Handle(object context, Func<object, Task> next)
        {
            // TODO: Update to use System.CommandLine 2.0 API when middleware support is available
            // if (context.ParseResult.Directives.Contains("apply"))
            // {
            //     var stopwatch = Stopwatch.StartNew();
            //
            //     await next(context);
            //
            //     stopwatch.Stop();
            //
            //     var logger = LoggingUtils.ConfigureLogger(null);
            //
            //     logger.Information("Completed in {time} ms", stopwatch.ElapsedMilliseconds);
            // }
            // else
            // {
                await next(context);
            // }
        }
    }
}
