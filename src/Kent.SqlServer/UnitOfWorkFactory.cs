namespace Kent.SqlServer
{
    using Kent.SqlServer.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Represents a type that used to create instances of <see cref="IUnitOfWork{TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type.</typeparam>
    public sealed class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        private readonly IOptionsMonitor<SqlConfiguration> _options;
        private readonly Dictionary<string, SqlConfiguration> _configs;
        private readonly IList<Type> _repositoryTypes;
        private readonly IDisposable _onChangeToken;
        private bool _disposed = false;

        /// <summary>
        ///     Constructor method.
        /// </summary>
        /// <param name="options">The options to create <see cref="DbContext"/> associated with <see cref="IUnitOfWork{TDbContext}"/> instances.</param>
        public UnitOfWorkFactory(IOptionsMonitor<SqlConfiguration> options) : this (options, nameof(SqlConfiguration))
        { }

        /// <summary>
        ///     Constructor method.
        /// </summary>
        /// <param name="options">The options to create <see cref="DbContext"/> associated with <see cref="IUnitOfWork{TDbContext}"/> instances.</param>
        /// <param name="name">The name of the options instance being configured.</param>
        public UnitOfWorkFactory(IOptionsMonitor<SqlConfiguration> options, string name)
        {
            _options = options;
            _configs = new Dictionary<string, SqlConfiguration>()
            {
                { name, _options.Get(name) }
            };
            _repositoryTypes = Enumerable.Empty<Type>().ToList();
            _onChangeToken = _options.OnChange(OptionsChanged);
        }

        /// <summary>
        ///     Creates the instance of <see cref="IUnitOfWork{TDbContext}"/>.
        /// </summary>
        /// <returns>An implementation of an unit of work.</returns>
        public IUnitOfWork<TDbContext> CreateUnitOfWork()
        {
            var configuration = GetConfig();
            var options = new DbContextOptionsBuilder<TDbContext>().UseSqlServer(configuration.ConnectionString, builder => builder.CommandTimeout(configuration.CommandTimeout)).Options;
            TDbContext context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), options);
            return new UnitOfWork<TDbContext>(context, _repositoryTypes.ToArray());
        }

        /// <summary>
        ///     Adds custom repositories to database context.<br/>
        ///     The custom repositories must be the implementation of <see cref="IRepository"/>
        /// </summary>
        /// <param name="repositoryTypes">An enumerator of arguments for repository types need to be added to unit of work.</param>
        public void AddCustomRepositories(IEnumerable<Type> repositoryTypes)
        {
            if (repositoryTypes == null)
            {
                throw new ArgumentNullException(nameof(repositoryTypes));
            }
            if (repositoryTypes.Any(t => !(t is IRepository)))
            {
                throw new ArgumentException("The custom repositories must be the implementation of IRepository");
            }

            foreach (var repositoryType in repositoryTypes)
            {
                if (!_repositoryTypes.Contains(repositoryType))
                {
                    _repositoryTypes.Add(repositoryType);
                }
            }
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
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _onChangeToken.Dispose();
                    _configs.Clear();
                }
            }
            _disposed = true;
        }

        private SqlConfiguration GetConfig() => _configs.First().Value;

        private void OptionsChanged(SqlConfiguration config, string name)
        {
            if (_configs.ContainsKey(name))
            {
                _configs[name] = config;
            }
        }

        IUnitOfWork IUnitOfWorkFactory.CreateUnitOfWork() => CreateUnitOfWork();
    }
}