namespace Kent.SqlServer
{
    using Kent.SqlServer.Abstractions;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents a type that can share one single database context when using multiple repositories.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type.</typeparam>
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly Dictionary<string, IRepository> _repositories;
        private bool _disposed = false;

        /// <summary>
        ///     Constructor method.
        /// </summary>
        /// <param name="context">The database context instance.</param>
        /// <param name="repositoryTypes">An array of arguments for repository types need to be added to unit of work.</param>
        public UnitOfWork(TDbContext context, params Type[] repositoryTypes)
        {
            _context = context;
            _repositories = new Dictionary<string, IRepository>();

            foreach (var entityType in _context.Model?.GetEntityTypes() ?? Enumerable.Empty<IEntityType>())
            {
                Type repositoryType = typeof(GenericRepository<>).MakeGenericType(entityType.ClrType);
                _repositories.Add(entityType.Name, (IRepository)Activator.CreateInstance(repositoryType, _context));
            }

            foreach (var repositoryType in repositoryTypes ?? Enumerable.Empty<Type>())
            {
                if (repositoryType.GetInterface(nameof(IRepository)) != null)
                {
                    var name = repositoryType.FullName;
                    if (!_repositories.ContainsKey(name))
                    {
                        _repositories.Add(name, (IRepository)Activator.CreateInstance(repositoryType, _context));
                    }
                }
            }
        }

        /// <summary>
        ///     Performs the command to create a new transaction.
        /// </summary>
        /// <param name="isolationLevel">The <see cref="IsolationLevel"/> to use.</param>
        /// <returns>A <see cref="IDbContextTransaction"/> that represents the started transaction.</returns>
        public IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return _context.Database?.BeginTransaction(isolationLevel);
        }

        /// <summary>
        ///     Performs the command to apply the operations in the current transacion.
        /// </summary>
        public void Commit()
        {
            _context.Database?.CommitTransaction();
        }

        /// <summary>
        ///     Performs the command to discard the operations in the current transacion.
        /// </summary>
        public void Rollback()
        {
            _context.Database?.RollbackTransaction();
        }

        /// <summary>
        ///     Performs the command to save all changes.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <summary>
        ///     Ensures that the database for the context exists.<br/>
        ///     If it does not exist then the database and all its schema are created.<br/>
        ///     If it does exist, no action is taken and no effort is made to ensure it is compatible with the model for this context.
        /// </summary>
        public void EnsureDatabaseCreated()
        {
            _context.Database.EnsureCreated();
        }

        /// <summary>
        ///     Ensures that the database for the context does not exist.<br/>
        ///     If it does exist then the database is deleted.<br/>
        ///     If it does not exist, no action is taken.
        /// </summary>
        public void EnsureDatabaseDeleted()
        {
            _context.Database.EnsureDeleted();
        }

        /// <summary>
        ///     Gets active entity repository, returns null if not found.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>An implementation of a repository.</returns>
        public IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class
        {
            var repository = _repositories.GetValueOrDefault(typeof(TEntity).FullName);
            if (repository != null)
            {
                return (IRepository<TEntity>)repository;
            }
            return default;
        }

        /// <summary>
        ///     Gets active repository, returns null if not found.
        /// </summary>
        /// <typeparam name="TRepository">The repository type.</typeparam>
        /// <returns>An implementation of a repository.</returns>
        public TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            var repository = _repositories.GetValueOrDefault(typeof(TRepository).FullName);
            if (repository != null)
            {
                return (TRepository)repository;
            }
            return default;
        }

        /// <summary>
        ///     Executes a command.
        /// </summary>
        /// <param name="commandText">The text represents the command.</param>
        /// <param name="parameters">The parameters that stored procedure takes.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<object> ExecuteCommand(string commandText, Dictionary<string, object> parameters = null)
        {
            return Task.Run(() =>
            {
                var result = new object();

                using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = transaction;
                                cmd.CommandText = commandText;
                                cmd.CommandTimeout = _context.Database.GetCommandTimeout() ?? cmd.CommandTimeout;
                                if (parameters != null)
                                {
                                    foreach (var parameter in parameters)
                                    {
                                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                                    }
                                }
                                result = cmd.ExecuteScalar();
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return result;
            });
        }

        /// <summary>
        ///     Executes a stored procedure using given name and parameters, then returns the result as target model.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <param name="procName">The name of the stored procedure that should be executed.</param>
        /// <param name="parameters">The parameters that stored procedure takes.</param>
        /// <param name="mapper">The function to do mapping between sql raw data to target model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<IEnumerable<TModel>> ExecuteStoredProcedure<TModel>(string procName, Dictionary<string, object> parameters = null, Func<Dictionary<string, object>, TModel> mapper = null) where TModel : class
        {
            return Task.Run(() =>
            {
                var result = Enumerable.Empty<TModel>();

                using (var conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            var ds = new DataSet();
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.Transaction = transaction;
                                cmd.CommandText = procName;
                                cmd.CommandTimeout = _context.Database.GetCommandTimeout() ?? cmd.CommandTimeout;
                                cmd.CommandType = CommandType.StoredProcedure;

                                if (parameters != null)
                                {
                                    foreach (var parameter in parameters)
                                    {
                                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                                    }
                                }
                                using (var da = new SqlDataAdapter(cmd))
                                {
                                    da.Fill(ds);
                                }
                            }
                            transaction.Commit();
                            if (ds.Tables.Count > 0)
                            {
                                result = ds.Tables[0].AsEnumerable().Select(row =>
                                {
                                    return mapper(row.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => (row[c] is DBNull ? null : row[c])));
                                }).ToList();
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return result;
            });
        }

        /// <summary>
        ///     Releases the allocated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases the allocated resources.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _repositories.Clear();
                }
            }
            _disposed = true;
        }
    }
}