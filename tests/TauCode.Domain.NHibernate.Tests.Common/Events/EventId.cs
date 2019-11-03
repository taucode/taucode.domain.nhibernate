using System;
using TauCode.Domain.Identities;

namespace TauCode.Domain.NHibernate.Tests.Common.Events
{
    [Serializable]
    public class EventId : IdBase
    {
        public EventId()
        {
        }

        public EventId(Guid id)
            : base(id)
        {
        }

        public EventId(string id)
            : base(id)
        {
        }
    }
}
