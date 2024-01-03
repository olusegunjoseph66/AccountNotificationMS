using System.Linq.Expressions;
using LinqKit;

namespace Shared.Data.Extensions
{
    public abstract class QueryObject<TEntity> : IQueryObject<TEntity>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Expression<Func<TEntity, bool>> _query;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual Expression<Func<TEntity, bool>> Expression => _query;

        public IQueryObject<TEntity> And(Expression<Func<TEntity, bool>> query)
        {
            _query = _query == null ? query : _query.And(query.Expand());
            return this;
        }

        public IQueryObject<TEntity> Or(Expression<Func<TEntity, bool>> query)
        {
            _query = _query == null ? query : _query.Or(query.Expand());
            return this;
        }

        public IQueryObject<TEntity> And(IQueryObject<TEntity> queryObject)
        {
            return And(queryObject.Expression);
        }

        public IQueryObject<TEntity> Or(IQueryObject<TEntity> queryObject)
        {
            return Or(queryObject.Expression);
        }
    }
}
