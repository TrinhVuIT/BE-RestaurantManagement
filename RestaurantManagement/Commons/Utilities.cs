using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace RestaurantManagement.Commons
{
    public static class Utilities
    {
        public static IQueryable<TSource> ApplyPaging<TSource>(this IEnumerable<TSource> source, int pageNo, int pageSize, out int totalItem)
        {
            totalItem = source.Count();
            return pageSize > 0 ? source.Skip((pageNo - 1) * pageSize).Take(pageSize).AsQueryable().AsAsyncQueryable() : ((IQueryable<TSource>)source).AsAsyncQueryable();
        }

        private static IQueryable<TEntity> AsAsyncQueryable<TEntity>(this IEnumerable<TEntity> source)
        => new AsyncQueryable<TEntity>(source ?? throw new ArgumentNullException(nameof(source)));

        private class AsyncQueryable<TEntity> : EnumerableQuery<TEntity>, IAsyncEnumerable<TEntity>, IQueryable<TEntity>
        {
            public AsyncQueryable(IEnumerable<TEntity> enumerable) : base(enumerable) { }
            public AsyncQueryable(Expression expression) : base(expression) { }
            public IAsyncEnumerator<TEntity> GetEnumerator() => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
            public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
            IQueryProvider IQueryable.Provider => new AsyncQueryProvider(this);

            private class AsyncEnumerator : IAsyncEnumerator<TEntity>
            {
                private readonly IEnumerator<TEntity> inner;
                public AsyncEnumerator(IEnumerator<TEntity> inner) => this.inner = inner;
                public void Dispose() => inner.Dispose();
                public TEntity Current => inner.Current;
                public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(inner.MoveNext());
#pragma warning disable CS1998 // Nothing to await
                public async ValueTask DisposeAsync() => inner.Dispose();
#pragma warning restore CS1998
            }

            private class AsyncQueryProvider : IAsyncQueryProvider
            {
                private readonly IQueryProvider inner;
                internal AsyncQueryProvider(IQueryProvider inner) => this.inner = inner;
                public IQueryable CreateQuery(Expression expression) => new AsyncQueryable<TEntity>(expression);
                public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new AsyncQueryable<TElement>(expression);
#pragma warning disable CS8603 // Possible null reference return.
                public object Execute(Expression expression) => inner.Execute(expression);
#pragma warning restore CS8603 // Possible null reference return.
                public TResult Execute<TResult>(Expression expression) => inner.Execute<TResult>(expression);
                public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) => new AsyncQueryable<TResult>(expression);
                TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Execute<TResult>(expression);
            }
        }
    }
}
