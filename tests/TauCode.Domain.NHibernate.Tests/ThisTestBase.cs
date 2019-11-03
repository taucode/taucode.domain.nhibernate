using NUnit.Framework;
using System;
using System.Data;
using TauCode.Domain.NHibernate.Tests.Base;

namespace TauCode.Domain.NHibernate.Tests
{
    [TestFixture]
    public abstract class ThisTestBase : TestBase
    {
        #region Overridden

        protected override TargetDbType GetTargetDbType() => TargetDbType.SQLite;
        protected override string CreateConnectionString()
        {
            throw new NotImplementedException();
        }

        protected override IDbConnection CreateDbConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
