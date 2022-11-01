namespace Kent.SqlServer.Tests.Repositories
{
    using Kent.SqlServer.Abstractions;
    using Kent.SqlServer.Tests.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class TestRepository : IRepository<Test>
    {
        public TestRepository(object context)
        {
            context.ToString();
        }

        public Task<IEnumerable<Test>> FindMany(Expression<Func<Test, bool>> filter = null, int? skip = null, int? take = null,
                                                Func<IQueryable<Test>, IOrderedQueryable<Test>> funcOrdering = null, string includeProperties = null)
        {
            return Task.Run(() => Enumerable.Empty<Test>());
        }

        public Task<IEnumerable<Test>> FindMany(Expression<Func<Test, bool>> filter = null, int? skip = null, int? take = null,
                                                string ordering = null, string includeProperties = null)
        {
            return Task.Run(() => Enumerable.Empty<Test>());
        }

        public Task<Test> FindOne(Expression<Func<Test, bool>> filter)
        {
            return Task.Run(() => new Test());
        }

        public Task<bool> InsertOne(Test entity)
        {
            return Task.Run(() => true);
        }

        public Task<bool> InsertMany(IEnumerable<Test> entities)
        {
            return Task.Run(() => true);
        }

        public Task<bool> DeleteOne(Test entity)
        {
            return Task.Run(() => true);
        }

        public Task<bool> DeleteMany(IEnumerable<Test> entities)
        {
            return Task.Run(() => true);
        }

        public Task<bool> UpdateOne(Test entity)
        {
            return Task.Run(() => true);
        }

        public Task<bool> UpdateMany(IEnumerable<Test> entities)
        {
            return Task.Run(() => true);
        }
    }
}