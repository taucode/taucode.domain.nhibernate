using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace TauCode.Domain.NHibernate.Types
{
    public class DateTimeAsOffset : IUserType
    {
        bool IUserType.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x?.GetHashCode() ?? 1599;
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var ordinal = rs.GetOrdinal(names[0]);

            if (rs.IsDBNull(ordinal))
            {
                return null;
            }

            var value = rs.GetValue(ordinal);

            if (value is DateTime dateTime)
            {
                var result = new DateTimeOffset(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond,
                    TimeSpan.Zero);

                return result;
            }

            throw new NotSupportedException($"Returned type '{value.GetType().FullName}' is not supported by {this.GetType().FullName}.");
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            DateTime paramValue;

            if (value is DateTimeOffset dateTimeOffset)
            {
                paramValue = dateTimeOffset.UtcDateTime;
            }
            else if (value is DateTime dateTime)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException();
            }

            var paramValueString = paramValue.ToString("u");
            paramValueString = paramValueString.Substring(0, paramValueString.Length - 1); // omit trailing 'Z'

            cmd.Parameters[index].Value = paramValueString;
        }

        public object DeepCopy(object value) => value;

        public object Replace(object original, object target, object owner)
        {
            throw new NotImplementedException();
        }

        public object Assemble(object cached, object owner)
        {
            throw new NotImplementedException();
        }

        public object Disassemble(object value)
        {
            throw new NotImplementedException();
        }

        public SqlType[] SqlTypes => new[] { new SqlType(DbType.DateTimeOffset), };
        public Type ReturnedType => typeof(DateTimeOffset);
        public bool IsMutable => false;
    }
}
