using System.Collections.Generic;

namespace TauCode.Domain.NHibernate.Tests.Common.Users
{
    public interface IUserRepository
    {
        User GetById(UserId id);
        IList<User> GetAll();
        void Save(User user);
    }
}
