using TauCode.Domain.Identities;
using TauCode.Domain.NHibernate.Types;

namespace TauCode.Domain.NHibernate.Tests
{
    public class DuzheSQLiteIdUserType<T> : SQLiteIdUserType<T> where T : IId
    {
    }
}
