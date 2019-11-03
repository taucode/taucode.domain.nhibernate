using NHibernate.Cfg;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SQLite;
using TauCode.Domain.NHibernate.Tests.Base;
using TauCode.Domain.NHibernate.Types;
using TauCode.Utils.Extensions;

namespace TauCode.Domain.NHibernate.Tests
{
    [TestFixture]
    public abstract class ThisTestBase : TestBase
    {
        #region Overridden

        protected override TargetDbType GetTargetDbType() => TargetDbType.SQLite;

        protected override string CreateConnectionString()
        {
            var path = FileExtensions.CreateTempFilePath("ztemp", ".sqlite");
            var connectionString = $"Data Source={path};Version=3;";
            return connectionString;
        }

        protected override IDbConnection CreateDbConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        protected override Type GetIdUserTypeGeneric() => typeof(SQLiteIdUserType<>);

        protected override Configuration CreateConfiguration(string connectionString)
        {
            var configuration = new Configuration();
            configuration.Properties.Add("connection.connection_string", connectionString);
            configuration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            configuration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");

            return configuration;
        }

        #endregion
    }
}
