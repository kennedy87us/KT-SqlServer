namespace Kent.SqlServer.Tests.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    public class AsyncEnumerable<TResult> : EnumerableQuery<TResult>, IAsyncEnumerable<TResult>, IQueryable<TResult>
    {
        public AsyncEnumerable(IEnumerable<TResult> enumerable) : base(enumerable)
        { }

        public AsyncEnumerable(Expression expression) : base(expression)
        { }

        public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator<TResult>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProvider<TResult>(this); }
        }
    }
}