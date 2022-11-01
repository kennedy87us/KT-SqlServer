namespace Kent.SqlServer.Abstractions
{
    using System;

    /// <summary>
    ///     Represents a type that used to create instances of <see cref="IUnitOfWork"/>.
    /// </summary>
    public interface IUnitOfWorkFactory : IDisposable
    {
        /// <summary>
        ///     Creates the instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns>An implementation of an unit of work.</returns>
        IUnitOfWork CreateUnitOfWork();
    }
}