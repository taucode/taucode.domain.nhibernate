using System;
using TauCode.Domain.Identities;

namespace TauCode.Domain.NHibernate.Tests.Domain.Users
{
    [Serializable]
    public class UserId : IdBase
    {
        public UserId()
        {
        }

        public UserId(Guid id)
            : base(id)
        {
        }

        public UserId(string id)
            : base(id)
        {
        }
    }
}
