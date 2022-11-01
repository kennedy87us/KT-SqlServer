namespace Kent.SqlServer.Tests.Infrastructure
{
    using Microsoft.EntityFrameworkCore.Query;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public AsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException($"{nameof(ExecuteAsync)} method has been cancelled", null, cancellationToken);
            object returnValue = Execute(expression);
            return ConvertToThreadingTResult<TResult>(returnValue);
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var returnValue = ExecuteAsync<TResult>(expression, cancellationToken);
            return ConvertToTResult<TResult>(returnValue);
        }

        private static TResult ConvertToThreadingTResult<TResult>(dynamic toConvert) => (TResult)Task.FromResult(toConvert);

        private static TResult ConvertToTResult<TResult>(dynamic toConvert) => (TResult)toConvert;
    }
}