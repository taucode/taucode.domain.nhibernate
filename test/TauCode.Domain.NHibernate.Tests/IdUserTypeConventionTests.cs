using NUnit.Framework;
using TauCode.Domain.NHibernate.Conventions;
using TauCode.Domain.NHibernate.Types;

namespace TauCode.Domain.NHibernate.Tests
{
    [TestFixture]
    public class IdUserTypeConventionTests
    {
        [Test]
        public void Constructor_IdUserType_RunsOk()
        {
            // Arrange

            // Act
            var convention = new IdUserTypeConvention(typeof(DuzheSQLiteIdUserType<>));

            // Assert
            // passed.
        }

        [Test]
        public void Constructor_SQLiteIdUserType_RunsOk()
        {
            // Arrange

            // Act
            var convention = new IdUserTypeConvention(typeof(SQLiteIdUserType<>));

            // Assert
            // passed.
        }

        [Test]
        public void Constructor_DuzheSQLiteIdUserType_RunsOk()
        {
            // Arrange

            // Act
            var convention = new IdUserTypeConvention(typeof(DuzheSQLiteIdUserType<>));

            // Assert
            // passed.
        }
    }
}
