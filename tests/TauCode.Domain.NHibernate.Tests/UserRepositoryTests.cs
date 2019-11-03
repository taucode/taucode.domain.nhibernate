using Autofac;
using NUnit.Framework;
using System;
using TauCode.Domain.NHibernate.Tests.Base;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests
{
    [TestFixture]
    public class UserRepositoryTests : ThisTestBase
    {
        private IUserRepository _testUserRepository;

        [SetUp]
        public void SetUp()
        {
            _testUserRepository = this.TestLifetimeScope.Resolve<IUserRepository>();
        }

        [Test]
        public void GetById_ExistingId_ReturnsUser()
        {
            // Arrange
            var id = TestHelper.AkId;

            // Act
            var ak = _testUserRepository.GetById(id);

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void GetById_NonExistingId_ReturnsNull()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void Save_NewUser_CreatesUser()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void Save_ExistingUser_UpdatesUser()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void Delete_ExistingUser_DeletesUser()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void Delete_NonExistingUser_DoesNothing()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }
    }
}
