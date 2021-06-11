using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MPS.Data.EF.Interfaces
{
    public interface IUpdateConfig<TEntity>
    {
        IUpdateConfig<TEntity> Exclude(params Expression<Func<TEntity, object>>[] xExpressions);
        IUpdateConfig<TEntity> OnlyInclude(params Expression<Func<TEntity, object>>[] xExpressions);
        IUpdateConfig<TEntity> UpdateRelations(params Expression<Func<TEntity, IEnumerable<object>>>[] xExpressions);

    }
}