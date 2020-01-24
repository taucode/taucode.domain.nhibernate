using NHibernate.Engine;
using System;
using System.Data;
using System.Data.Common;
using TauCode.Domain.Identities;

namespace TauCode.Domain.NHibernate.Types
{
    /// <summary>
    /// .NET Core implementation of SQLite (NuGet package System.Data.SQLite) is weird: if you define
    /// CREATE TABLE my_table(
    ///     id UNIQUEIDENTIFIER PRIMARY KEY,
    ///     name VARCHAR(100)
    /// ),
    /// and then query this table via DbCommand/DbReader, 'id' column is retrieved as Guid (which is nice);
    /// but if you add a 'WHERE id = @p_id' condition, and provide '@p_id' parameter as a Guid representing an existing id,
    /// query returns 0 rows instead of 1.
    ///
    /// If, however, you provide that parameter as a Guid-like string representing the same existing id, query returns exactly the row you want.
    /// Therefore, we need to substitute command's parameter with string instead keeping IId which uses Guid under the hood.
    /// </summary>
    /// <typeparam name="T">Identity type</typeparam>
    public class SQLiteIdUserType<T> : IdUserType<T> where T : IId
    {
        public override void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            object parameterValue;

            if (value == null)
            {
                parameterValue = DBNull.Value;
            }
            else
            {
                if (value is IId id)
                {
                    parameterValue = id.ToString();
                }
                else if (value is Guid guid)
                {
                    // necessary for HQL, e.g. there is a NHibernate method "SetGuid" but there is no "SetId"
                    parameterValue = guid.ToString();
                }
                else if (value is string str)
                {
                    parameterValue = str;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            cmd.Parameters[index].DbType = DbType.AnsiString;
            cmd.Parameters[index].Value = parameterValue;
        }
    }
}
