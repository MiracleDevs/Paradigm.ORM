using System.Threading.Tasks;
using Paradigm.ORM.DbPublisher.Builders;

namespace Paradigm.ORM.DbPublisher.Runners
{
    public interface IScriptRunner
    {
        Task RunAsync(IScriptBuilder scriptBuilder);
    }
}