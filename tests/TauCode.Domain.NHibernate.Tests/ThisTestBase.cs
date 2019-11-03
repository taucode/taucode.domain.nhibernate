using NUnit.Framework;
using System.Data;
using System.Data.SQLite;
using TauCode.Domain.NHibernate.Tests.Base;
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

        #endregion
    }
}
