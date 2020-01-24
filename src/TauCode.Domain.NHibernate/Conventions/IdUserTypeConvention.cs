using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using TauCode.Domain.Identities;
using TauCode.Domain.NHibernate.Types;

namespace TauCode.Domain.NHibernate.Conventions
{
    public class IdUserTypeConvention : IPropertyConvention, IIdConvention
    {
        private readonly Type _idUserTypeGeneric;

        public IdUserTypeConvention(Type idUserTypeGeneric)
        {
            if (idUserTypeGeneric == null)
            {
                throw new ArgumentNullException(nameof(idUserTypeGeneric));
            }

            var isValid = HasIdUserTypeGenericInHierarchy(idUserTypeGeneric);

            if (!isValid)
            {
                throw new ArgumentException(
                    $"'{nameof(idUserTypeGeneric)}' must represent a type 'SomeIdUserType<>' where 'SomeIdUserType' is defined as 'public class SomeIdUserType<T> : IdUserType<T> where T : IId'",
                    nameof(idUserTypeGeneric));
            }

            _idUserTypeGeneric = idUserTypeGeneric;
        }

        public void Apply(IPropertyInstance instance)
        {
            var propertyType = instance.Type.GetUnderlyingSystemType();
            var identityUserType = this.GetUserType(propertyType);

            if (identityUserType == null)
            {
                return;
            }

            instance.CustomType(identityUserType);
        }

        public void Apply(IIdentityInstance instance)
        {
            var propertyType = instance.Type.GetUnderlyingSystemType();
            var identityUserType = this.GetUserType(propertyType);

            if (identityUserType == null)
            {
                return;
            }

            instance.CustomType(identityUserType);
        }

        private Type GetUserType(Type propertyType)
        {
            // This convention only applies to identity properties
            if (!typeof(IId).IsAssignableFrom(propertyType))
            {
                return null;
            }

            Type[] typeArgs = { propertyType };
            var identityUserType = _idUserTypeGeneric.MakeGenericType(typeArgs);

            return identityUserType;
        }

        private static bool HasIdUserTypeGenericInHierarchy(Type idUserTypeGeneric)
        {
            var curr = idUserTypeGeneric;

            while (true)
            {
                if (curr == null)
                {
                    return false;
                }

                if (curr == typeof(IdUserType<>))
                {
                    return true;
                }

                var ancestor = curr.BaseType;
                if (ancestor == null)
                {
                    return false;
                }

                if (!ancestor.IsGenericType)
                {
                    return false;
                }

                curr = ancestor.GetGenericTypeDefinition();
            }
        }
    }
}
