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
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class GenericRepositoryTests
    {
        private readonly Mock<DbSet<Test>> _mockSet;
        private readonly Mock<DbContext> _mockContext;
        private readonly Mock<IUnitOfWorkFactory<DbContext>> _mockUnitOfWorkFactory;
        private IQueryable<Test> _data;

        public GenericRepositoryTests()
        {
            InitData();

            _mockSet = new Mock<DbSet<Test>>();
            _mockSet.As<IAsyncEnumerable<Test>>().Setup(m => m.GetAsyncEnumerator(default))
                                                 .Returns(new AsyncEnumerator<Test>(_data.GetEnumerator()));
            _mockSet.As<IQueryable<Test>>().Setup(m => m.Provider)
                                           .Returns(new AsyncQueryProvider<Test>(_data.Provider));
            _mockSet.As<IQueryable<Test>>().Setup(m => m.Expression)
                                           .Returns(_data.Expression);
            _mockSet.As<IQueryable<Test>>().Setup(m => m.ElementType)
                                           .Returns(_data.ElementType);
            _mockSet.As<IQueryable<Test>>().Setup(m => m.GetEnumerator())
                                           .Returns(_data.GetEnumerator());

            _mockContext = new Mock<DbContext>();
            _mockContext.Setup(m => m.Model.GetEntityTypes()).Returns(new List<IEntityType> { new EntityType(typeof(Test)) });
            _mockContext.Setup(m => m.Set<Test>()).Returns(_mockSet.Object);
            _mockContext.Setup(m => m.Entry(It.IsAny<Test>()));

            _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory<DbContext>>();
            _mockUnitOfWorkFactory.Setup(m => m.CreateUnitOfWork()).Returns(new UnitOfWork<DbContext>(_mockContext.Object, typeof(TestRepository)));
        }

        private void InitData()
        {
            _data = new List<Test>
            {
                new Test { Id = "id1", Value = "value1", RefId = "refId1" },
                new Test { Id = "id2", Value = "value2", RefId = "refId2" },
                new Test { Id = "id3", Value = "value3", RefId = "refId2" }
            }.AsQueryable();
        }

        [Theory]
        [InlineData("id1", "id2", "id3")]
        public async Task FindMany_NoCondition_ReturnAllRecords(params object[] expected)
        {
            //Arrange
            IEnumerable<Test> result = Enumerable.Empty<Test>();

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.FindMany(t => true, funcOrdering: null);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Length, result.Count());
            Assert.Collection(result,
                t => { Assert.Equal(expected.ElementAt(0), t.Id); },
                t => { Assert.Equal(expected.ElementAt(1), t.Id); },
                t => { Assert.Equal(expected.ElementAt(2), t.Id); }
            );
        }

        [Theory]
        [InlineData("refId2", "id2", "id3")]
        public async Task FindMany_WithCondition_ReturnManyRecords(string condition, params object[] expected)
        {
            //Arrange
            IEnumerable<Test> result = Enumerable.Empty<Test>();

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.FindMany(t => t.RefId == condition, funcOrdering: null);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Length, result.Count());
            Assert.Collection(result,
                t => { Assert.Equal(expected.ElementAt(0), t.Id); },
                t => { Assert.Equal(expected.ElementAt(1), t.Id); }
            );
        }

        [Theory]
        [InlineData("refId")]
        public async Task FindMany_WithCondition_ReturnNoRecord(string condition)
        {
            //Arrange
            IEnumerable<Test> result = Enumerable.Empty<Test>();

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.FindMany(t => t.RefId == condition, funcOrdering: null);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("id1", "value1", "refId1")]
        [InlineData("id2", "value2", "refId2")]
        [InlineData("id3", "value3", "refId2")]
        public async Task FindOne_WithCondition_ReturnOneRecord(string key, string expected1, string expected2)
        {
            //Arrange
            Test result = null;

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.FindOne(t => t.Id == key);
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expected1, result.Value);
            Assert.Equal(expected2, result.RefId);
        }

        [Theory]
        [InlineData("id")]
        public async Task FindOne_WithCondition_ReturnNoRecord(string key)
        {
            //Arrange
            Test result = null;

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.FindOne(t => t.Id == key);
            }

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task InsertMany_NoException_ReturnTrue()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "test1", Value = "test1Value" },
                new Test { Id = "test2", Value = "test2Value" }
            };

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.InsertMany(entities);
            }

            //Assert
            _mockSet.Verify(m => m.AddAsync(It.IsAny<Test>(), default), Times.Exactly(2));
            Assert.True(result);
        }

        [Fact]
        public async Task InsertMany_ThrowException_ReturnFalse()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "test1", Value = "test1Value" },
                new Test { Id = "test2", Value = "test2Value" }
            };
            _mockSet.Setup(m => m.AddAsync(It.IsAny<Test>(), default)).Throws<Exception>();

            //Act
            try
            {
                using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
                {
                    var repository = unitOfWork.GetEntityRepository<Test>();
                    result = await repository.InsertMany(entities);
                }
            }
            catch { }

            //Assert
            _mockSet.Verify(m => m.AddAsync(It.IsAny<Test>(), default), Times.Once());
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteMany_NoException_ReturnTrue()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "id1", Value = "value1" },
                new Test { Id = "id2", Value = "value2" }
            };

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.DeleteMany(entities);
            }

            //Assert
            _mockSet.Verify(m => m.Remove(It.IsAny<Test>()), Times.Exactly(2));
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteMany_ThrowException_ReturnFalse()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "id1", Value = "value1" },
                new Test { Id = "id2", Value = "value2" }
            };
            _mockSet.Setup(m => m.Remove(It.IsAny<Test>())).Throws<Exception>();

            //Act
            try
            {
                using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
                {
                    var repository = unitOfWork.GetEntityRepository<Test>();
                    result = await repository.DeleteMany(entities);
                }
            }
            catch { }

            //Assert
            _mockSet.Verify(m => m.Remove(It.IsAny<Test>()), Times.Once());
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateMany_NoException_ReturnTrue()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "id1", Value = "value1updated" },
                new Test { Id = "id2", Value = "value2updated" }
            };

            //Act
            using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
            {
                var repository = unitOfWork.GetEntityRepository<Test>();
                result = await repository.UpdateMany(entities);
            }

            //Assert
            _mockSet.Verify(m => m.Attach(It.IsAny<Test>()), Times.Exactly(2));
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateMany_ThrowException_ReturnFalse()
        {
            //Arrange
            var result = default(bool);
            var entities = new List<Test> {
                new Test { Id = "id1", Value = "value1updated" },
                new Test { Id = "id2", Value = "value2updated" }
            };
            _mockSet.Setup(m => m.Attach(It.IsAny<Test>())).Throws<Exception>();

            //Act
            try
            {
                using (var unitOfWork = _mockUnitOfWorkFactory.Object.CreateUnitOfWork())
                {
                    var repository = unitOfWork.GetEntityRepository<Test>();
                    result = await repository.UpdateMany(entities);
                }
            }
            catch { }

            //Assert
            _mockSet.Verify(m => m.Attach(It.IsAny<Test>()), Times.Once());
            Assert.False(result);
        }
    }
}