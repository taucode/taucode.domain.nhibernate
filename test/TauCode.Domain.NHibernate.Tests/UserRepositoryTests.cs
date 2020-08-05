using Autofac;
using NUnit.Framework;
using System.Linq;
using TauCode.Domain.NHibernate.Tests.Base;
using TauCode.Domain.NHibernate.Tests.Domain.Users;
using TauCode.Domain.NHibernate.Tests.Persistence.Repositories;

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

        protected override void BuildContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<NHibernateUserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
        }

        [Test]
        public void GetById_ExistingId_ReturnsUser()
        {
            // Arrange
            var id = TestHelper.AkId;

            // Act
            var ak = _testUserRepository.GetById(id);

            // Assert
            Assert.That(ak, Is.Not.Null);
            Assert.That(ak.Id, Is.EqualTo(new UserId("ca2c8f0a-00eb-49a3-b495-cd14fe830b0f")));
            Assert.That(ak.Name, Is.EqualTo("ak"));
        }

        [Test]
        public void GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var id = new UserId(TestHelper.NonExistingId);

            // Act
            var user = _testUserRepository.GetById(id);

            // Assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public void GetAll_NoArguments_ReturnsAllUser()
        {
            // Arrange

            // Act
            var users = _testUserRepository.GetAll();

            // Assert
            Assert.That(users, Has.Count.EqualTo(3));

            var user = users.Single(x => x.Id == TestHelper.AkId);
            Assert.That(user.Name, Is.EqualTo("ak"));

            user = users.Single(x => x.Id == TestHelper.OliaId);
            Assert.That(user.Name, Is.EqualTo("olia"));

            user = users.Single(x => x.Id == TestHelper.IraId);
            Assert.That(user.Name, Is.EqualTo("ira"));
        }

        [Test]
        public void Save_NewUser_CreatesUser()
        {
            // Arrange
            var deserea = new User("deserea");
            var id = deserea.Id;

            // Act
            this.TestSession.DoInTransaction(() =>
            {
                _testUserRepository.Save(deserea);
            });

            // Assert
            var loaded = this.AssertSession.Load<User>(id);
            Assert.That(loaded.Id, Is.EqualTo(id));
            Assert.That(loaded.Name, Is.EqualTo("deserea"));
        }

        [Test]
        public void Save_ExistingUser_UpdatesUser()
        {
            // Arrange
            var id = TestHelper.AkId;
            var ak = this.SetupSession.Load<User>(id);

            // Act
            ak.ChangeName("andy");
            this.TestSession.DoInTransaction(() =>
            {
                _testUserRepository.Save(ak);
            });

            // Assert
            var updated = this.AssertSession.Load<User>(id);
            Assert.That(updated.Id, Is.EqualTo(id));
            Assert.That(updated.Name, Is.EqualTo("andy"));
        }

        [Test]
        public void Delete_ExistingUser_DeletesUser()
        {
            // Arrange
            var id = TestHelper.IraId;

            // Act
            bool? deleted = null;
            this.TestSession.DoInTransaction(() =>
            {
                deleted = _testUserRepository.Delete(id);
            });

            // Assert
            Assert.That(deleted, Is.True);
            var remainingIds = this.AssertSession
                .Query<User>()
                .ToList()
                .Select(x => x.Id);
            CollectionAssert.AreEquivalent(remainingIds, new[] { TestHelper.AkId, TestHelper.OliaId });
        }

        [Test]
        public void Delete_NonExistingUser_DoesNothing()
        {
            // Arrange
            var id = new UserId(TestHelper.NonExistingId);

            // Act
            bool? deleted = null;
            this.TestSession.DoInTransaction(() =>
            {
                deleted = _testUserRepository.Delete(id);
            });

            // Assert
            Assert.That(deleted, Is.False);
            var remainingIds = this.AssertSession
                .Query<User>()
                .ToList()
                .Select(x => x.Id);
            CollectionAssert.AreEquivalent(
                remainingIds,
                new[]
                {
                TestHelper.AkId,
                TestHelper.OliaId,
                TestHelper.IraId,
                });
        }
    }
}
