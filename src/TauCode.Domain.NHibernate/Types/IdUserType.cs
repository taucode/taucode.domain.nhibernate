using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using TauCode.Domain.Identities;

namespace TauCode.Domain.NHibernate.Types
{
    public class IdUserType<T> : IUserType where T : IId
    {
        protected readonly ConstructorInfo GuidCtor;

        public IdUserType()
        {
            this.GuidCtor = typeof(T).GetConstructor(new[] { typeof(Guid) });
            if (this.GuidCtor == null)
            {
                throw new ArgumentException($"Type '{typeof(T).FullName}' doesn't have public .ctor(Guid).");
            }
        }

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
            return x?.GetHashCode() ?? typeof(T).GetHashCode() + 1599;
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var ordinal = rs.GetOrdinal(names[0]);

            if (rs.IsDBNull(ordinal))
            {
                return null;
            }

            var guid = TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(rs[names[0]].ToString());
            var id = this.GuidCtor.Invoke(new[] { guid });
            return id;
        }

        public virtual void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            object paramValue;

            if (value == null)
            {
                paramValue = DBNull.Value;
            }
            else
            {
                paramValue = TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(value.ToString());
            }

            cmd.Parameters[index].Value = paramValue;
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
