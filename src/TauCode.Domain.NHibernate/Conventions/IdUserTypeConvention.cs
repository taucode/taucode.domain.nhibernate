using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using TauCode.Domain.Identities;
using TauCode.Domain.NHibernate.Types;

namespace TauCode.Domain.NHibernate.Conventions
{
    public class IdUserTypeConvention : IPropertyConvention, IIdConvention
    {
        public IdUserTypeConvention(Type idUserTypeGeneric)
        {
            if (idUserTypeGeneric == null)
            {
                throw new ArgumentNullException(nameof(idUserTypeGeneric));
            }

            var valid = false;

            do
            {
                if (!idUserTypeGeneric.IsConstructedGenericType)
                {
                    break;
                }

                var underlyingType = idUserTypeGeneric.GetGenericTypeDefinition();
                throw new NotImplementedException();

            } while (false);

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

            var genericIdentityUserType = typeof(IdUserType<>);
            Type[] typeArgs = { propertyType };
            var identityUserType = genericIdentityUserType.MakeGenericType(typeArgs);

            return identityUserType;
        }
    }
}
