using System;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Domain.Events
{
    public class Event
    {
        private Event()
        {
        }

        public Event(UserId userId, string description)
        {
            this.Id = new EventId();
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public EventId Id { get; private set; }
        public UserId UserId { get; private set; }
        public string Description { get; private set; }
    }
}
