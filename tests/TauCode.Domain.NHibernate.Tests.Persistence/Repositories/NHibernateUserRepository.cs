using NHibernate;
using System.Collections.Generic;
using System.Linq;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Persistence.Repositories
{
    public class NHibernateUserRepository : IUserRepository
    {
        private readonly ISession _session;

        public NHibernateUserRepository(ISession session)
        {
            _session = session;
        }

        public User GetById(UserId id)
        {
            var user = _session
                .Query<User>()
                .SingleOrDefault(x => x.Id == id);

            return user;
        }

        public IList<User> GetAll()
        {
            var users = _session
                .Query<User>()
                .ToList();

            return users;
        }

        public void Save(User user)
        {
            _session.SaveOrUpdate(user);
        }
    }
}
