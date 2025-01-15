using DatabaseControl.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseControl.Tests
{
    [TestFixture]
    internal class ServiceCollectionExtensionsTests
    {
        private IServiceCollection services;

        [SetUp]
        public void SetUp()
        {
            services = new ServiceCollection();

            services.AddDbContextFactory<TestDbContext>();
        }

        [Test]
        public void AddRepository_ShouldRegisterDatabaseRepositoryAsSingleton()
        {
            // Act
            services.AddRepository<TestDbContext>();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var repository = serviceProvider.GetService<IDatabaseRepository<TestDbContext>>();

            Assert.NotNull(repository);
            Assert.That(repository, Is.TypeOf<DatabaseRepository<TestDbContext>>());
        }

        [Test]
        public void AddDbContextFactory_ShouldRegisterDbContextFactory()
        {
            // Arrange
            const string connectionString = "Host=localhost;Database=testdb;Username=testuser;Password=testpassword";

            // Act
            services.AddDbContextFactory<TestDbContext>(connectionString);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<IDbContextFactory<TestDbContext>>();

            Assert.NotNull(factory);
            Assert.That(factory, Is.InstanceOf<IDbContextFactory<TestDbContext>>());
        }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }
}