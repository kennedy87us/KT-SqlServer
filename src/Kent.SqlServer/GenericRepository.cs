namespace Kent.SqlServer
{
    using Kent.SqlServer.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents a generic entity repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        ///     Constructor method.
        /// </summary>
        /// <param name="context">The database context instance.</param>
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        ///     Finds the entities matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <param name="skip">The number of entities to skip before returning the remaining ones.</param>
        /// <param name="take">The number of entities to return.</param>
        /// <param name="funcOrdering">The delegate function to do sorting operation.</param>
        /// <param name="includeProperties">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<IEnumerable<TEntity>> FindMany(Expression<Func<TEntity, bool>> filter = null, int? skip = null, int? take = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> funcOrdering = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            //filter
            if (filter != null) { query = query.Where(filter); }

            //include
            foreach (string includeProperty in includeProperties?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>())
            {
                query = query.Include(includeProperty);
            }

            //ordering
            if (funcOrdering != null) { query = funcOrdering(query); }

            //skip
            if (skip.HasValue && skip > 0) { query = query.Skip((int)skip); }

            //take
            if (take.HasValue && take > 0) { query = query.Take((int)take); }

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///     Finds the entities matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <param name="skip">The number of entities to skip before returning the remaining ones.</param>
        /// <param name="take">The number of entities to return.</param>
        /// <param name="ordering">An expression string to indicate values to order by.</param>
        /// <param name="includeProperties">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<IEnumerable<TEntity>> FindMany(Expression<Func<TEntity, bool>> filter = null, int? skip = null, int? take = null,
                                                         string ordering = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            //filter
            if (filter != null) { query = query.Where(filter); }

            //include
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            //ordering
            if (!string.IsNullOrEmpty(ordering)) { query = query.OrderBy(ordering); }

            //skip
            if (skip.HasValue && skip > 0) { query = query.Skip((int)skip); }

            //take
            if (take.HasValue && take > 0) { query = query.Take((int)take); }

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///     Finds an entity matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<TEntity> FindOne(Expression<Func<TEntity, bool>> filter)
        {
            var entity = default(TEntity);
            await Task.Run(() =>
            {
                entity = _dbSet.SingleOrDefault(filter);
                if (entity != null)
                {
                    var entry = _context.Entry(entity);     // work around unit test issue
                    if (entry != null)
                    {
                        entry.State = EntityState.Detached;
                    }
                }
            });
            return entity;
        }

        /// <summary>
        ///     Inserts a single entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> InsertOne(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        /// <summary>
        ///     Inserts many entities.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> InsertMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await InsertOne(entity);
            }
            return true;
        }

        /// <summary>
        ///     Deletes a single entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> DeleteOne(TEntity entity)
        {
            await Task.Run(() =>
            {
                var entry = _context.Entry(entity);     // work around unit test issue
                if (entry != null && entry.State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            });
            return true;
        }

        /// <summary>
        ///     Deletes many entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> DeleteMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteOne(entity);
            }
            return true;
        }

        /// <summary>
        ///     Updates a single entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> UpdateOne(TEntity entity)
        {
            await Task.Run(() =>
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);     // work around unit test issue
                if (entry != null)
                {
                    entry.State = EntityState.Modified;
                }
            });
            return true;
        }

        /// <summary>
        ///     Updates many entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>A task whose returned result.</returns>
        public async Task<bool> UpdateMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateOne(entity);
            }
            return true;
        }
    }
}