using System;
using System.Collections.Generic;
using System.Linq;

namespace Postback.Blog.App.Data
{
    public interface IPersistenceSession
    {
        void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();
        void Delete<T>(T item) where T : class, new();
        void DeleteAll<T>() where T : class, new();
        T FindOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();
        T Get<T>(string id) where T : class, new();
        IQueryable<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();
        IQueryable<T> All<T>() where T : class, new();
        T Save<T>(T item) where T : class, new();
        void Save<T>(IEnumerable<T> items) where T : class, new();
    }
}