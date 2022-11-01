namespace Kent.SqlServer.Abstractions
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     Represents a type that used to create instances of <see cref="IUnitOfWork{TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type.</typeparam>
    public interface IUnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory where TDbContext : DbContext
    {
        /// <summary>
        ///     Creates the instance of <see cref="IUnitOfWork{TDbContext}"/>.
        /// </summary>
        /// <returns>An implementation of an unit of work.</returns>
        new IUnitOfWork<TDbContext> CreateUnitOfWork();
    }
}