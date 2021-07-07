using System.Threading.Tasks;
using Paradigm.ORM.DbPublisher.Builders;

namespace Paradigm.ORM.DbPublisher.Runners
{
    public interface IScriptRunner
    {
        /// <summary>
        /// Runs the scripts.
        /// </summary>
        /// <param name="scriptBuilder">The script builder.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        /// <returns></returns>
        Task RunAsync(IScriptBuilder scriptBuilder, bool verbose);
    }
}