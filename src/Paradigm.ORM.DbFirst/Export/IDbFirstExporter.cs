using System.Threading.Tasks;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Export
{
    internal interface IDbFirstExporter
    {
        Task ExportAsync(string configurationFileName, DbFirstConfiguration configuration);
    }
}