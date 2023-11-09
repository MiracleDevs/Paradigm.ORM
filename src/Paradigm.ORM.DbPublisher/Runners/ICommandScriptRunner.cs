using System.Threading.Tasks;
using Paradigm.ORM.DbPublisher.Builders;

namespace Paradigm.ORM.DbPublisher.Runners
{
    public interface ICommandScriptRunner
    {
        /// <summary>
        /// Runs the scripts.
        /// </summary>
        /// <param name="scriptBuilder">The script builder.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns></returns>
        Task RunAsync(ICommandScriptBuilder scriptBuilder, bool verbose);
    }
}