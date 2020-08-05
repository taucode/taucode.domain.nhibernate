using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using Inflector;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using TauCode.Db;
using TauCode.Db.SQLite;
using TauCode.Db.SqlServer;
using TauCode.Domain.NHibernate.Conventions;
using TauCode.Domain.NHibernate.Tests.Persistence;
using TauCode.Extensions;

namespace TauCode.Domain.NHibernate.Tests.Base
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IDbConnection Connection { get; private set; }
        protected string ConnectionString { get; private set; }
        protected IDbInspector DbInspector { get; private set; }
        protected IScriptBuilder ScriptBuilder { get; private set; }
        protected ICruder Cruder { get; private set; }

        protected IDbSerializer DbSerializer { get; private set; }

        protected IContainer Container { get; private set; }

        protected ILifetimeScope SetupLifetimeScope { get; private set; }
        protected ILifetimeScope TestLifetimeScope { get; private set; }
        protected ILifetimeScope AssertLifetimeScope { get; private set; }

        protected ISession SetupSession { get; private set; }
        protected ISession TestSession { get; private set; }
        protected ISession AssertSession { get; private set; }

        protected abstract string GetDbProviderName();
        protected abstract string CreateConnectionString();
        protected abstract IDbConnection CreateDbConnection(string connectionString);

        protected virtual IDbInspector CreateDbInspector(IDbConnection connection)
        {
            var dbType = this.GetDbProviderName();
            switch (dbType)
            {
                case DbProviderNames.SqlServer:
                    return new SqlServerInspector(connection);

                case DbProviderNames.SQLite:
                    return new SQLiteInspector(connection);

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual IScriptBuilder CreateScriptBuilder()
        {
            var dbType = this.GetDbProviderName();
            switch (dbType)
            {
                case DbProviderNames.SqlServer:
                    return new SqlServerScriptBuilder();

                case DbProviderNames.SQLite:
                    return new SQLiteScriptBuilder();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual ICruder CreateCruder()
        {
            var dbType = this.GetDbProviderName();
            switch (dbType)
            {
                case DbProviderNames.SqlServer:
                    return new SqlServerCruder(this.Connection);

                case DbProviderNames.SQLite:
                    return new SQLiteCruder(this.Connection);

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual IDbSerializer CreateDataSerializer()
        {
            var dbType = this.GetDbProviderName();
            switch (dbType)
            {
                case DbProviderNames.SqlServer:
                    return new SqlServerSerializer(this.Connection);

                case DbProviderNames.SQLite:
                    return new SQLiteSerializer(this.Connection);

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected abstract Type GetIdUserTypeGeneric();

        protected abstract Configuration CreateConfiguration(string connectionString);

        protected abstract void BuildContainer(ContainerBuilder containerBuilder);

        private ISessionFactory BuildSessionFactory(Configuration configuration, Assembly mappingsAssembly)
        {
            return Fluently.Configure(configuration)
                .Mappings(m => m.FluentMappings.AddFromAssembly(mappingsAssembly)
                    .Conventions.Add(ForeignKey.Format((p, t) =>
                    {
                        if (p == null) return t.Name.Underscore() + "_id";

                        return p.Name.Underscore() + "_id";
                    }))
                    .Conventions.Add(LazyLoad.Never())
                    .Conventions.Add(Table.Is(x => x.TableName.Underscore().ToUpper()))
                    .Conventions.Add(ConventionBuilder.Property.Always(x => x.Column(x.Property.Name.Underscore())))
                    .Conventions.Add(typeof(IdUserTypeConvention), new IdUserTypeConvention(this.GetIdUserTypeGeneric()))
                )
                .BuildSessionFactory();
        }

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");

            this.ConnectionString = this.CreateConnectionString();

            var containerBuilder = new ContainerBuilder();
            var configuration = this.CreateConfiguration(this.ConnectionString);
            var mappingsAssembly = typeof(PersistenceBeacon).Assembly;

            containerBuilder.Register(c => BuildSessionFactory(configuration, mappingsAssembly))
                .As<ISessionFactory>()
                .SingleInstance();

            containerBuilder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerLifetimeScope();

            this.BuildContainer(containerBuilder);

            this.Container = containerBuilder.Build();

            this.Connection = this.CreateDbConnection(this.ConnectionString);
            this.Connection.Open();
            this.DbInspector = this.CreateDbInspector(this.Connection);
            this.ScriptBuilder = this.CreateScriptBuilder();
            this.Cruder = this.CreateCruder();
            this.DbSerializer = this.CreateDataSerializer();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
                this.Connection = null;
            }
        }

        [SetUp]
        public void SetUpBase()
        {
            // autofac stuff
            this.SetupLifetimeScope = this.Container.BeginLifetimeScope();
            this.TestLifetimeScope = this.Container.BeginLifetimeScope();
            this.AssertLifetimeScope = this.Container.BeginLifetimeScope();

            // nhibernate stuff
            this.SetupSession = this.SetupLifetimeScope.Resolve<ISession>();
            this.TestSession = this.TestLifetimeScope.Resolve<ISession>();
            this.AssertSession = this.AssertLifetimeScope.Resolve<ISession>();

            // data
            this.DbInspector.DropAllTables();
            var script = typeof(TestBase).Assembly.GetResourceText("create-db.sql", true);
            this.Connection.ExecuteCommentedScript(script);
            
            var json = typeof(TestBase).Assembly.GetResourceText("testdb.json", true);
            this.DbSerializer.DeserializeDbData(json);
        }

        [TearDown]
        public void TearDownBase()
        {
            this.SetupSession.Dispose();
            this.TestSession.Dispose();
            this.AssertSession.Dispose();

            this.SetupLifetimeScope.Dispose();
            this.TestLifetimeScope.Dispose();
            this.AssertLifetimeScope.Dispose();
        }
    }
}
