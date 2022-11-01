namespace Kent.SqlServer.Extensions
{
    using Kent.SqlServer.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Specifies the extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds SQL Server database context, configuration and unit of work factory.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            return AddSqlServer<TDbContext>(services, nameof(SqlConfiguration), () => Enumerable.Empty<Type>());
        }

        /// <summary>
        ///     Adds SQL Server database context, configuration and unit of work factory.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection services, string name) where TDbContext : DbContext
        {
            return AddSqlServer<TDbContext>(services, name, () => Enumerable.Empty<Type>());
        }

        /// <summary>
        ///     Adds SQL Server database context, configuration and unit of work factory.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="funcRepositoryTypes">The function returns the repositories need to be registered to unit of work.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection services, Func<IEnumerable<Type>> funcRepositoryTypes) where TDbContext : DbContext
        {
            return AddSqlServer<TDbContext>(services, nameof(SqlConfiguration), funcRepositoryTypes);
        }

        /// <summary>
        ///     Adds SQL Server database context, configuration and unit of work factory.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="funcRepositoryTypes">The function returns repository types need to be registered to unit of work.</param>
        /// <param name="injectBaseFactory">true to be injected as the base of unit of work factory; otherwise, false.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSqlServer<TDbContext>(this IServiceCollection services, string name, Func<IEnumerable<Type>> funcRepositoryTypes, bool injectBaseFactory = true) where TDbContext : DbContext
        {
            if (injectBaseFactory)
            {
                services.AddSingleton<IUnitOfWorkFactory>(p =>
                {
                    var options = p.GetService<IOptionsMonitor<SqlConfiguration>>();
                    var unitOfWorkFactory = new UnitOfWorkFactory<TDbContext>(options, name);
                    unitOfWorkFactory.AddCustomRepositories(funcRepositoryTypes?.Invoke() ?? Enumerable.Empty<Type>());
                    return unitOfWorkFactory;
                });
            }
            services.TryAddSingleton<IUnitOfWorkFactory<TDbContext>>(p =>
            {
                var options = p.GetService<IOptionsMonitor<SqlConfiguration>>();
                var unitOfWorkFactory = new UnitOfWorkFactory<TDbContext>(options, name);
                unitOfWorkFactory.AddCustomRepositories(funcRepositoryTypes?.Invoke() ?? Enumerable.Empty<Type>());
                return unitOfWorkFactory;
            });

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.Configure<SqlConfiguration>(name, configuration.GetSection(name));

            return services;
        }
    }
}