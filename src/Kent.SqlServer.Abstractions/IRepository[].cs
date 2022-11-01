namespace Kent.SqlServer.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents an entity repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        /// <summary>
        ///     Finds the entities matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <param name="skip">The number of entities to skip before returning the remaining ones.</param>
        /// <param name="take">The number of entities to return.</param>
        /// <param name="funcOrdering">The delegate function to do sorting operation.</param>
        /// <param name="includeProperties">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A task whose returned result.</returns>
        Task<IEnumerable<TEntity>> FindMany(Expression<Func<TEntity, bool>> filter = null, int? skip = null, int? take = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> funcOrdering = null, string includeProperties = null);

        /// <summary>
        ///     Finds the entities matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <param name="skip">The number of entities to skip before returning the remaining ones.</param>
        /// <param name="take">The number of entities to return.</param>
        /// <param name="ordering">An expression string to indicate values to order by.</param>
        /// <param name="includeProperties">A string of '.' separated navigation property names to be included.</param>
        /// <returns>A task whose returned result.</returns>
        Task<IEnumerable<TEntity>> FindMany(Expression<Func<TEntity, bool>> filter = null, int? skip = null, int? take = null,
                                            string ordering = null, string includeProperties = null);

        /// <summary>
        ///     Finds an entity matching the filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <returns>A task whose returned result.</returns>
        Task<TEntity> FindOne(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        ///     Inserts a single entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> InsertOne(TEntity entity);

        /// <summary>
        ///     Inserts many entities.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> InsertMany(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Deletes a single entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> DeleteOne(TEntity entity);

        /// <summary>
        ///     Deletes many entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> DeleteMany(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Updates a single entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> UpdateOne(TEntity entity);

        /// <summary>
        ///     Updates many entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>A task whose returned result.</returns>
        Task<bool> UpdateMany(IEnumerable<TEntity> entities);
    }
}