using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Translation
{
    public abstract class TranslatorBase<TInput, TOutput> : ITranslator<TInput, TOutput>
    {
        protected IDatabaseConnector Connector { get; }

        protected DbFirstConfiguration Configuration { get; }

        protected TranslatorBase(IDatabaseConnector connector, DbFirstConfiguration configuration)
        {
            this.Connector = connector;
            this.Configuration = configuration;
        }

        public abstract TOutput Translate(TInput input);
    }
}