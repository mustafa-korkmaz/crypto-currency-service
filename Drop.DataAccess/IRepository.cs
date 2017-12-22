using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.DataAccess
{
    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> predicate);
        IQueryable<T> AsQueryable();
        DbRawSqlQuery<T> RawSql(string sql);

        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
