using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

namespace DentistApp.DAL.DAL
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : BaseEntity
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            Func<T, bool> exp = t => t.IsDeleted == false;
            List<T> list;
            using (var context = new DentistDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(exp)
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            Func<T, bool> expr1 = T => T.IsDeleted == false;
            Func<T, bool> expr2 = T => where(T) && expr1(T);

            List<T> list;
            using (var context = new DentistDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(expr2)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            Func<T, bool> expr1 = T=>T.IsDeleted==false;
            Func<T, bool> expr2 = T=>where(T) && expr1(T);
                        
            using (var context = new DentistDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(expr2); //Apply where clause
            }
            return item;
        }

        /* rest of code omitted */


        public int Add(T item)
        {
            using (var context = new DentistDbContext())
            {
                DbSet<T> dbSet = context.Set<T>();

                item.EntityState = EntityState.Added;
                dbSet.Add(item);
                foreach (DbEntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
                {
                    IEntity entity = entry.Entity;
                    entry.State = GetEntityState(entity.EntityState);
                }

                return context.SaveChanges();
            }

        }

        public int Update(params T[] items)
        {

            using (var context = new DentistDbContext())
            {

                context.Configuration.ValidateOnSaveEnabled = true;
                DbSet<T> dbSet = context.Set<T>();
                foreach (T item in items)
                {
                    item.EntityState = EntityState.Modified;
                    dbSet.Attach(item); 
                    
                    foreach (DbEntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
                    {
                        IEntity entity = entry.Entity;
                        entry.State = GetEntityState(entity.EntityState);
                    }

                }

                context.SaveChanges();
            }

            return items.Length;

        }

        public int Remove(params T[] items)
        {
            using (var context = new DentistDbContext())
            {
                foreach (var item in items)
                {
                    item.EntityState = EntityState.Modified;
                    item.IsDeleted = true;
                    context.Entry(item).State = GetEntityState(item.EntityState);
                }
                return context.SaveChanges();
            }
        }

        protected static System.Data.Entity.EntityState GetEntityState(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Unchanged:
                    return System.Data.Entity.EntityState.Unchanged;
                case EntityState.Added:
                    return System.Data.Entity.EntityState.Added;
                case EntityState.Modified:
                    return System.Data.Entity.EntityState.Modified;
                case EntityState.Deleted:
                    return System.Data.Entity.EntityState.Deleted;
                default:
                    return System.Data.Entity.EntityState.Detached;
            }
        }
    }
}
