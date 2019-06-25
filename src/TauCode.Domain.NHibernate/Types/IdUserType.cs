using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace TauCode.Domain.NHibernate.Types
{
    public class IdUserType<T> : IUserType
    {
        bool IUserType.Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) { return true; }
            if (x == null || y == null) { return false; }
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x?.GetHashCode() ?? typeof(T).GetHashCode() + 1599;
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var ordinal = rs.GetOrdinal(names[0]);
            if (rs.IsDBNull(ordinal)) return null;
            var obj = TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(rs[names[0]].ToString());
            return typeof(T).GetConstructor(new[] { typeof(Guid) }).Invoke(new[] { obj });
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            ((IDataParameter)cmd.Parameters[index]).Value = value != null ?
                TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(value.ToString()) : DBNull.Value;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get
            {
                Enum.TryParse(typeof(Guid).Name, false, out DbType dbType);
                return new[] { new SqlType(dbType) };
            }
        }
        public Type ReturnedType => typeof(T);
        public bool IsMutable => false;
    }
}
