namespace Kent.SqlServer.Tests.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncEnumerator<TResult> : IAsyncEnumerator<TResult>
    {
        private readonly IEnumerator<TResult> _inner;

        public AsyncEnumerator(IEnumerator<TResult> inner)
        {
            _inner = inner;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(_inner.MoveNext());
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Run(() => _inner.Dispose());
        }

        public TResult Current
        {
            get { return _inner.Current; }
        }
    }
}