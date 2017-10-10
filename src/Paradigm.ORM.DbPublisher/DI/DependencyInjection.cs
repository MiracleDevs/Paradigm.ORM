using Paradigm.Core.DependencyInjection;
using Paradigm.Core.DependencyInjection.Interfaces;
using Paradigm.ORM.DbPublisher.Builders;
using Paradigm.ORM.DbPublisher.Configuration;
using Paradigm.ORM.DbPublisher.Logging;
using Paradigm.ORM.DbPublisher.Runners;

namespace Paradigm.ORM.DbPublisher.DI
{
    public static class DependencyInjection
    {
        public static IDependencyContainer Register(PublishConfiguration configuration)
        {
            var builder = DependencyBuilderFactory.Create(DependencyLibrary.Microsoft);

            builder.RegisterInstance(ConnectorBuilder.Build(configuration));
            builder.RegisterInstance<ILoggingService>(new ConsoleLoggingService());
            builder.Register<IScriptBuilder, ScriptBuilder>();
            builder.Register<IScriptRunner, ScriptRunner>();

            return builder.Build();
        }
    }
}