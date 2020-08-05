using FluentNHibernate.Mapping;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Persistence.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            this.Table("[user]");
            this.Id(x => x.Id);
            this.Map(x => x.Name);
        }
    }
}
