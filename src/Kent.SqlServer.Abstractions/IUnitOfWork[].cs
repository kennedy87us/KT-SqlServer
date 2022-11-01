namespace Kent.SqlServer.Abstractions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents a type that can share one single database context when using multiple repositories.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type.</typeparam>
    public interface IUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        /// <summary>
        ///     Performs the command to create a new transaction.
        /// </summary>
        /// <param name="isolationLevel">The <see cref="IsolationLevel"/> to use.</param>
        /// <returns>A <see cref="IDbContextTransaction"/> that represents the started transaction.</returns>
        new IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        ///     Performs the command to apply the operations in the current transacion.
        /// </summary>
        new void Commit();

        /// <summary>
        ///     Performs the command to discard the operations in the current transacion.
        /// </summary>
        new void Rollback();

        /// <summary>
        ///     Performs the command to save all changes.
        /// </summary>
        new void Save();

        /// <summary>
        ///     Ensures that the database for the context exists.<br/>
        ///     If it does not exist then the database and all its schema are created.<br/>
        ///     If it does exist, no action is taken and no effort is made to ensure it is compatible with the model for this context.
        /// </summary>
        new void EnsureDatabaseCreated();

        /// <summary>
        ///     Ensures that the database for the context does not exist.<br/>
        ///     If it does exist then the database is deleted.<br/>
        ///     If it does not exist, no action is taken.
        /// </summary>
        new void EnsureDatabaseDeleted();

        /// <summary>
        ///     Gets active entity repository, returns null if not found.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        new IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class;

        /// <summary>
        ///     Gets active repository, returns null if not found.
        /// </summary>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        new TRepository GetRepository<TRepository>() where TRepository : IRepository;

        /// <summary>
        ///     Executes a command.
        /// </summary>
        /// <param name="commandText">The text represents the command.</param>
        /// <param name="parameters">The parameters that stored procedure takes.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        new Task<object> ExecuteCommand(string commandText, Dictionary<string, object> parameters = null);

        /// <summary>
        ///     Executes a stored procedure using given name and parameters, then returns the result as target model.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <param name="procName">The name of the stored procedure that should be executed.</param>
        /// <param name="parameters">The parameters that stored procedure takes.</param>
        /// <param name="mapper">The function to do mapping between sql raw data to target model.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        new Task<IEnumerable<TModel>> ExecuteStoredProcedure<TModel>(string procName, Dictionary<string, object> parameters = null, Func<Dictionary<string, object>, TModel> mapper = null) where TModel : class;
    }
}