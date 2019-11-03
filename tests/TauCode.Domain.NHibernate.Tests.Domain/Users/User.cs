using System;

namespace TauCode.Domain.NHibernate.Tests.Domain.Users
{
    public class User
    {
        private User()
        {
        }

        public User(string name)
        {
            this.Id = new UserId();
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public UserId Id { get; private set; }
        public string Name { get; private set; }
    }
}
