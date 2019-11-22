using System.Collections.Generic;

namespace TauCode.Domain.NHibernate.Tests.Domain.Users
{
    public interface IUserRepository
    {
        User GetById(UserId id);
        IList<User> GetAll();
        void Save(User user);
        bool Delete(UserId id);
    }
}
