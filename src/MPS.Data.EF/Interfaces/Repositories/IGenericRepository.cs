using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moba.Data.EF.Helpers;
using Moba.Data.EF.Services;

namespace Moba.Data.EF.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region --SyncFunctions--

        int Count(Expression<Func<TEntity, bool>> filter = null);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null);

        IEnumerable<TViewModel> Get<TViewModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null);

        IEnumerable<TEntity> Get(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null,
            int page = 0,
            int pageSize = 0);

        bool Any(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

        IEnumerable<TViewModel> Get<TViewModel>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null,
            int page = 0,
            int pageSize = 0);


        IEnumerable<TEntity> Get<TValue>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0) where TValue : struct;

        IEnumerable<TViewModel> Get<TValue, TViewModel>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0) where TValue : struct;

        TEntity GetById(object id);

        TEntity GetFirst(
            Expression<Func<TEntity, bool>> xWherePredicate,
            MapBy<TEntity> includeProperties = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        TViewModel GetFirst<TViewModel>(
            Expression<Func<TEntity, bool>> xWherePredicate,
            MapBy<TViewModel> includeProperties = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        void Insert(TEntity entity);
        #endregion

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate, Action<IUpdateConfig<TEntity>> xConfiguration = null);

        void UpdateProperty<TResult>(TEntity xBaseEntity, // Select Base Entity
                                   params Expression<Func<TEntity, IEnumerable<TResult>>>[] xRelations // Select many-to-many relationships
                                   ) where TResult : class;

        #region --AsyncFunctions--
        Task<List<TEntity>> GetAsync(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             MapBy<TEntity> includeProperties = null);


        Task<List<TViewModel>> GetAsync<TViewModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null);


        Task<PagingResult<TEntity>> GetAsync(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null,
            int page = 0,
            int pageSize = 0);

        Task<PagingResult<TViewModel>> GetAsync<TViewModel>(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null,
            int page = 0,
            int pageSize = 0);

        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(object id);

        Task<PagingResult<TEntity>> GetAsync<TValue>(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0) where TValue : struct;

        Task<PagingResult<TViewModel>> GetAsync<TValue, TViewModel>(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0) where TValue : struct;

        Task InsertAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> xWherePredicate, MapBy<TEntity> includeProperties = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TViewModel> GetFirstAsync<TViewModel>(Expression<Func<TEntity, bool>> xWherePredicate, MapBy<TViewModel> includeProperties = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IGenericRepository<TEntity> EnableTracking();
        IGenericRepository<TEntity> DisableTracking();
        #endregion
    }
}