using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DentistApp.DAL.DAL
{
    public interface IGenericDataRepository<T> where T : BaseEntity
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        int Add(T item);
        int Update(params T[] items);
        int Remove(params T[] items);
    }
}
