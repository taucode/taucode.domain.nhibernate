using System.Collections.Generic;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Domain.Events
{
    public interface IEventRepository
    {
        Event GetById(EventId id);
        IList<Event> GetByUserId(UserId userId);
        void Save(Event @event);
    }
}
