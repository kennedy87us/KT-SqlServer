namespace Kent.SqlServer.Tests
{
    using Kent.SqlServer.Abstractions;
    using Kent.SqlServer.Tests.Infrastructure;
    using Kent.SqlServer.Tests.Models;
    using Kent.SqlServer.Tests.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Moq;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class UnitOfWorkTests
    {
        private readonly Mock<DbContext> _mockContext;
        private readonly Mock<IUnitOfWorkFactory<DbContext>> _mockUnitOfWorkFactory;

        public UnitOfWorkTests()
        {
            _mockContext = new Mock<DbContext>();
            _mockContext.Setup(m => m.Model.GetEntityTypes()).Returns(new List<IEntityType> { new EntityType(typeof(Test)) });

            _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory<DbContext>>();
            _mockUnitOfWorkFactory.Setup(m => m.CreateUnitOfWork()).Returns(new UnitOfWork<DbContext>(_mockContext.Object));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ApplyTransaction(bool throwException)
        {
            //Arrange

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                try
                {
                    unitOfWork.CreateTransaction();
                    unitOfWork.Save();
                    if (throwException)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                }
            }

            //Assert
            _mockContext.VerifyGet(m => m.Database, Times.Exactly(2));
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void GetEntityRepository()
        {
            //Arrange
            IRepository repository = null;

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                repository = unitOfWork.GetEntityRepository<Test>();
            }

            //Assert
            Assert.NotNull(repository);
            Assert.True(repository is IRepository<Test>);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetRepository(bool registerRepository)
        {
            //Arrange
            IRepository repository = null;
            if (registerRepository)
            {
                _mockUnitOfWorkFactory.Setup(m => m.CreateUnitOfWork()).Returns(new UnitOfWork<DbContext>(_mockContext.Object, typeof(TestRepository)));
            }

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                repository = unitOfWork.GetRepository<TestRepository>();
            }

            //Assert
            if (registerRepository)
            {
                Assert.NotNull(repository);
                Assert.True(repository is IRepository<Test>);
            }
            else
            {
                Assert.Null(repository);
            }
        }
    }
}