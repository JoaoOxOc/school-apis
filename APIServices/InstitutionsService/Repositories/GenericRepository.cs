using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InstitutionsService.Services.UnitOfWork;
using Utils.Enums;
using Utils.Extensions;
using Utils.Interfaces;

namespace InstitutionsService.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DatabaseContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DatabaseContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            // We will use reflection to set the metadata properties of the entity
            Type t = entity.GetType();

            PropertyInfo createdAt = t.GetProperty("CreatedAt");
            PropertyInfo updatedAt = t.GetProperty("UpdatedAt");

            if (createdAt != null && createdAt.CanWrite)
            {
                createdAt.SetValue(entity, DateTime.Now, null);
            }

            if (updatedAt != null && updatedAt.CanWrite)
            {
                updatedAt.SetValue(entity, DateTime.Now, null);
            }

            dbSet.Add(entity);
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            int resultsCount = filter != null ? query.Where(filter).Count() : query.Count();

            return resultsCount;
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
        }

        public List<TEntity> Get(int? pageNum = null,
            int? size = null, Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> sort = null,
            SortDirection sortDirection = SortDirection.Ascending,
            string includeProperties = "", bool noTracking = false)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (sort != null)
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    query = query.OrderBy(sort);
                }
                else
                {
                    query = query.OrderByDescending(sort);
                }
            }
            else
            {
                query = query.OrderBy("Id", SortDirection.Ascending);
            }

            if (pageNum.HasValue && size.HasValue)
            {
                query = query.Skip((pageNum.Value - 1) * size.Value).Take(size.Value);
            }

            if (noTracking)
                return query.AsNoTracking().ToList();
            else
                return query.ToList();
        }

        public List<TEntity> Get(int? pageNum = null,
            int? size = null, Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            string includeProperties = "", bool noTracking = false)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy, sortDirection);
            }
            else
            {
                // By default this will implement the order by the Id -- this is needed for the linq Skip method to work
                query = query.OrderBy("Id", SortDirection.Ascending);
            }

            if (pageNum.HasValue && size.HasValue)
            {
                query = query.Skip((pageNum.Value - 1) * size.Value).Take(size.Value);
            }

            if (noTracking)
                return query.AsNoTracking().ToList<TEntity>();
            else
                return query.ToList<TEntity>();
        }

        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void Update(TEntity entityToUpdate)
        {
            Type t = entityToUpdate.GetType();
            PropertyInfo updatedAt = t.GetProperty("UpdatedAt");

            if (updatedAt != null)
            {
                updatedAt.SetValue(entityToUpdate, DateTime.Now, null);
            }

            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
