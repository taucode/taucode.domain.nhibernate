using Autofac;
using NHibernate;
using NUnit.Framework;
using System;
using System.Data;
using System.Globalization;
using TauCode.Db.Lab.Utils.Serialization.SQLite;
using TauCode.Db.Utils.Building;
using TauCode.Db.Utils.Building.SQLite;
using TauCode.Db.Utils.Building.SqlServer;
using TauCode.Db.Utils.Crud;
using TauCode.Db.Utils.Crud.SQLite;
using TauCode.Db.Utils.Crud.SqlServer;
using TauCode.Db.Utils.Inspection;
using TauCode.Db.Utils.Inspection.SQLite;
using TauCode.Db.Utils.Inspection.SqlServer;
using TauCode.Db.Utils.Serialization;
using TauCode.Db.Utils.Serialization.SqlServer;

namespace TauCode.Domain.NHibernate.Tests.Base
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IDbConnection Connection { get; private set; }
        protected string ConnectionString { get; private set; }
        protected IDbInspector DbInspector { get; private set; } // todo: get rid of.
        protected IScriptBuilder ScriptBuilder { get; private set; } // todo: get rid of.
        protected ICruder Cruder { get; private set; } // todo: get rid of.

        protected IDataSerializer DataSerializer { get; private set; }

        protected IContainer Container { get; private set; }

        protected ILifetimeScope SetupLifetimeScope { get; private set; }
        protected ILifetimeScope TestLifetimeScope { get; private set; }
        protected ILifetimeScope AssertLifetimeScope { get; private set; }

        protected ISession SetupSession { get; private set; }
        protected ISession TestSession { get; private set; }
        protected ISession AssertSession { get; private set; }

        protected abstract TargetDbType GetTargetDbType();
        protected abstract string CreateConnectionString();
        protected abstract IDbConnection CreateDbConnection(string connectionString);

        protected virtual IDbInspector CreateDbInspector(IDbConnection connection)
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerInspector(connection);

                case TargetDbType.SQLite:
                    return new SQLiteInspector(connection);

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual IScriptBuilder CreateScriptBuilder()
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerScriptBuilder();

                case TargetDbType.SQLite:
                    return new SQLiteScriptBuilder();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual ICruder CreateCruder()
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerCruder();

                case TargetDbType.SQLite:
                    return new SQLiteCruder();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        protected virtual IDataSerializer CreateDataSerializer()
        {
            var dbType = this.GetTargetDbType();
            switch (dbType)
            {
                case TargetDbType.SqlServer:
                    return new SqlServerDataSerializer();

                case TargetDbType.SQLite:
                    return new SQLiteDataSerializerLab();

                default:
                    throw new NotSupportedException($"{dbType} is not supported.");
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");

            var containerBuilder = new ContainerBuilder();


            this.Container = containerBuilder.Build();
            this.ConnectionString = this.CreateConnectionString();

            this.Connection = this.CreateDbConnection(this.ConnectionString);
            this.Connection.Open();
            this.DbInspector = this.CreateDbInspector(this.Connection);
            this.ScriptBuilder = this.CreateScriptBuilder();
            this.Cruder = this.CreateCruder();
            this.DataSerializer = this.CreateDataSerializer();
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
    }
}
