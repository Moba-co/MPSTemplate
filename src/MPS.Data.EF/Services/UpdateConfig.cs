using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moba.Data.EF.Interfaces;

namespace Moba.Data.EF.Services
{
    public class UpdateConfig<TEntity> : IUpdateConfig<TEntity>
    {
        public UpdateConfig()
        {
            Excludes = new List<Expression<Func<TEntity, object>>>();
            OnlyIncludes = new List<Expression<Func<TEntity, object>>>();
            Relations = new List<string>();
        }
        internal List<Expression<Func<TEntity, object>>> Excludes { set; get; }
        internal List<Expression<Func<TEntity, object>>> OnlyIncludes { set; get; }
        internal List<string> Relations { private set; get; }

        public IUpdateConfig<TEntity> Exclude(params Expression<Func<TEntity, object>>[] xExpressions)
        {
            Excludes.AddRange(xExpressions);
            return this;

        }
        public IUpdateConfig<TEntity> OnlyInclude(params Expression<Func<TEntity, object>>[] xExpressions)
        {
            OnlyIncludes.AddRange(xExpressions);
            return this;

        }
        public IUpdateConfig<TEntity> UpdateRelations(params Expression<Func<TEntity, IEnumerable<object>>>[] xExpressions)
        {
            Relations.AddRange(xExpressions.Select(s => s.Body.ToString().Split('.')[1]));
            return this;
        }
    }
}