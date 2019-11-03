using System;
using System.Collections.Generic;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Persistence.Repositories
{
    public class NHibernateUserRepository : IUserRepository
    {
        public User GetById(UserId id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save(User user)
        {
            throw new NotImplementedException();
        }
    }
}
