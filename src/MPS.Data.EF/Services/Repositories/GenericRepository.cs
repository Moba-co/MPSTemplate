using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MPS.Data.EF.Context;
using MPS.Data.EF.Helpers;
using MPS.Data.EF.Interfaces;
using MPS.Data.EF.Interfaces.Repositories;

namespace MPS.Data.EF.Services.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        #region --Ctro And Init Context--
        internal MPSDbContext context;
        internal DbSet<TEntity> dbSet;
        internal IMapper _mapper;
        private bool IsTrackingEnabled = false;

        public GenericRepository(MPSDbContext context, IMapper mapper)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }
        #endregion

        #region --Sync Function--

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter != null ? dbSet.Count(filter) : dbSet.Count();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            return filter != null ? query.Any(filter) : query.Any();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                return await query.AnyAsync(filter);

            return await query.AnyAsync();
        }

        public virtual IQueryable<TEntity> Query(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                MapBy<TEntity> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }

            if (orderBy != null)
                query = orderBy(query);

            return IsTrackingEnabled ? query : query.AsNoTracking();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }

            if (IsTrackingEnabled)
                return orderBy == null ? query : orderBy(query);
            return orderBy == null ? query.AsNoTracking() : orderBy(query).AsNoTracking();
        }

        public virtual IEnumerable<TEntity> Get(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null,
            int page = 0,
            int pageSize = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (listCount != -1) { listCount = query.Count(); }

            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }
            return IsTrackingEnabled ? query : query.AsNoTracking();
        }

        public virtual IEnumerable<TViewModel> Get<TValue, TViewModel>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0
            ) where TValue : struct
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (listCount != -1)
                listCount = query.Count();

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (groupBy != null)
            {
                query = query.GroupBy(groupBy).SelectMany(p => p.Take(take));
            }

            if (limit != 0)
            {
                query = query.Take(limit);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider);
        }

        public virtual IEnumerable<TEntity> Get<TValue>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0
            ) where TValue : struct
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (listCount != -1)
                listCount = query.Count();

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (groupBy != null)
            {

                query = query.GroupBy(groupBy).SelectMany(p => p.Take(take));
            }

            if (limit != 0)
            {
                query = query.Take(limit);
            }
            return IsTrackingEnabled ? query : query.AsNoTracking();
        }

        public virtual IEnumerable<TViewModel> Get<TViewModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null)
        {

            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= new MapBy<TViewModel>();

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return orderBy != null ? orderBy(query).ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray())
                : query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray());
        }

        public virtual IEnumerable<TViewModel> Get<TViewModel>(
            ref int listCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null,
            int page = 0,
            int pageSize = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (listCount != -1) { listCount = query.Count(); }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            includeProperties ??= new MapBy<TViewModel>();

            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray());
        }




        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetFirst(
            Expression<Func<TEntity, bool>> xWherePredicate,
            MapBy<TEntity> includeProperties = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var query = dbSet.Where(xWherePredicate);
            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return IsTrackingEnabled ? query.FirstOrDefault() : query.AsNoTracking().FirstOrDefault();
        }

        public virtual TViewModel GetFirst<TViewModel>(
            Expression<Func<TEntity, bool>> xWherePredicate,
            MapBy<TViewModel> includeProperties = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            includeProperties ??= new MapBy<TViewModel>();
            var query = dbSet.Where(xWherePredicate);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray()).FirstOrDefault();
        }


        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate, Action<IUpdateConfig<TEntity>> configuration = null)
        {
            dbSet.Attach(entityToUpdate);
            var entry = context.Entry(entityToUpdate);
            entry.State = EntityState.Modified;


            var updateConfig = new UpdateConfig<TEntity>();
            if (configuration == null) return;
            configuration(updateConfig);

            // var excludePropertiesFromUpdate = updateConfig.Excludes;
            // var onlyIncludeProperties = updateConfig.OnlyIncludes;
            var relations = updateConfig.Relations;

            foreach (var relation in relations)
            {
                var newData = entry.Collection(relation).CurrentValue;
                entry.Collection(relation).CurrentValue = null;

                //remove trackers:
                var currentValue = newData as object[] ?? newData.Cast<object>().ToArray();
                foreach (var i in currentValue)
                {
                    context.Entry(i).State = EntityState.Detached;
                }
                // get fresh data from db
                entry.Collection(relation).Load();
                entry.Collection(relation).CurrentValue = currentValue;
            }

            // if (onlyIncludeProperties.Any())
            // {
            //     var onlyIncludedProps = onlyIncludeProperties.Select(s => entry.Property(s).Metadata.Name).ToList();
            //     foreach (var excludeProp in entry.Properties.Where(w => !onlyIncludedProps.Contains(w.Metadata.Name)))
            //     {
            //         excludeProp.IsModified = false;
            //     }
            //     if (excludePropertiesFromUpdate.Any())
            //         throw new Exception("Cant use OnlyInclude with exclude at the same time/");
            // }
            // else if (excludePropertiesFromUpdate.Any())
            // {
            //
            //     foreach (var curExcludeColumn in excludePropertiesFromUpdate)
            //     {
            //         entry.Property(curExcludeColumn).IsModified = false;
            //     }
            // }

        }


        public virtual void UpdateProperty<TResult>(TEntity xBaseEntity, // Select Base Entity
                                   params Expression<Func<TEntity, IEnumerable<TResult>>>[] xRelations // Select many-to-many relationships
                                   ) where TResult : class
        {

            var entry = dbSet.Attach(xBaseEntity);

            entry.State = EntityState.Unchanged;

            foreach (var xRelation in xRelations)
            {
                var newData = entry.Collection(propertyExpression: xRelation).CurrentValue.ToList();
                entry.Collection(propertyExpression: xRelation).CurrentValue = null;

                //remove trackers:
                foreach (var i in newData)
                {
                    context.Entry(i).State = EntityState.Detached;
                }
                // get fresh data from db
                entry.Collection(propertyExpression: xRelations.First()).Load();
                entry.Collection(propertyExpression: xRelations.First()).CurrentValue = newData;
            }
        }

        public IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }
        #endregion

        #region --AsyncFunctions--

        public virtual async Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null)
        {

            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }


        public virtual async Task<List<TViewModel>> GetAsync<TViewModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null)
        {

            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            includeProperties ??= new MapBy<TViewModel>();


            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            if (orderBy != null)
            {
                return await orderBy(query).ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray())
                    .ToListAsync();
            }
            return await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray()).ToListAsync();
        }


        public virtual async Task<PagingResult<TEntity>> GetAsync(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TEntity> includeProperties = null,
            int page = 0,
            int pageSize = 0)
        {
            IQueryable<TEntity> query = dbSet;
            var pagingGenericResult = new PagingResult<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (needDbCount) { pagingGenericResult.Count = query.Count(); }

            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            pagingGenericResult.Data = await query.ToListAsync();
            return pagingGenericResult;
        }


        public virtual async Task<PagingResult<TViewModel>> GetAsync<TViewModel>(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            MapBy<TViewModel> includeProperties = null,
            int page = 0,
            int pageSize = 0)
        {
            IQueryable<TEntity> query = dbSet;
            var pagingGenericResult = new PagingResult<TViewModel>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (needDbCount) { pagingGenericResult.Count = query.Count(); }


            if (orderBy != null)
            {
                query = orderBy(query);
            }

            includeProperties ??= new MapBy<TViewModel>();

            if (page != 0 && pageSize != 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            pagingGenericResult.Data = await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray()).ToListAsync();
            return pagingGenericResult;
        }

        public virtual async Task<PagingResult<TEntity>> GetAsync<TValue>(
            bool needDbCount,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, TValue>> groupBy = null,
            int take = 0,
            int limit = 0
            ) where TValue : struct
        {
            var pagingGenericResult = new PagingResult<TEntity>();
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (needDbCount) { pagingGenericResult.Count = query.Count(); }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (groupBy != null)
            {

                query = query.GroupBy(groupBy).SelectMany(p => p.Take(take));
            }

            if (limit != 0)
            {
                query = query.Take(limit);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            pagingGenericResult.Data = await query.ToListAsync();
            return pagingGenericResult;
        }

        public virtual async Task<PagingResult<TViewModel>> GetAsync<TValue, TViewModel>(
              bool needDbCount,
              Expression<Func<TEntity, bool>> filter = null,
              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
              Expression<Func<TEntity, TValue>> groupBy = null,
              int take = 0,
              int limit = 0
              ) where TValue : struct
        {
            var pagingGenericResult = new PagingResult<TViewModel>();
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (needDbCount)
            {
                pagingGenericResult.Count = query.Count();
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (groupBy != null)
            {
                query = query.GroupBy(groupBy).SelectMany(p => p.Take(take));
            }

            if (limit != 0)
            {
                query = query.Take(limit);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            pagingGenericResult.Data = await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return pagingGenericResult;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(delegate { Delete(entity); });
        }

        public async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> xWherePredicate, MapBy<TEntity> includeProperties = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var query = dbSet.Where(xWherePredicate);
            if (includeProperties != null)
            {
                query = includeProperties.GetIncludeProperties().Aggregate(query, (current, item) => current.Include(item));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TViewModel> GetFirstAsync<TViewModel>(Expression<Func<TEntity, bool>> xWherePredicate,
            MapBy<TViewModel> includeProperties = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            includeProperties ??= new MapBy<TViewModel>();

            var query = dbSet.Where(xWherePredicate);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!IsTrackingEnabled)
                query = query.AsNoTracking();

            return await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider, null, includeProperties.GetIncludeProperties().ToArray()).FirstOrDefaultAsync();
        }


        //protected static Expression<Func<TEntity, bool>> EqualsPredicate<T>(T id)
        //{

        //    Expression<Func<IEntity<T>, T>> selector = x => x.xID;
        //    Expression<Func<T>> closure = () => id;
        //    return Expression.Lambda<Func<TEntity, bool>>(
        //        Expression.Equal(selector.Body, closure.Body),
        //        selector.Parameters);
        //}
        #endregion

        #region --Other Functions--
        public IGenericRepository<TEntity> EnableTracking()
        {
            IsTrackingEnabled = true;
            return this;
        }

        public IGenericRepository<TEntity> DisableTracking()
        {
            IsTrackingEnabled = false;
            return this;
        }
        #endregion
        // context is our ObjectContext


    }
}