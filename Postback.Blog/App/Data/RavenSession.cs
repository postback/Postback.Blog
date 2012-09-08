using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;

namespace Postback.Blog.App.Data
{
    public class RavenSession : IPersistenceSession
    {
        protected IDocumentSession documentSession;

        public RavenSession(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            var items = All<T>().Where(expression);
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : class, new()
        {
            documentSession.Delete(item);
            documentSession.SaveChanges();
        }

        public void DeleteAll<T>() where T : class, new()
        {
            var items = All<T>();
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return documentSession.Query<T>().SingleOrDefault(expression);
        }

        public IQueryable<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return documentSession.Query<T>().Where(expression);
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return documentSession.Query<T>().AsQueryable();
        }

        public T Add<T>(T item) where T : class, new()
        {
            documentSession.Store(item);
            documentSession.SaveChanges();

            return item;
        }

        public void Save<T>(T item) where T : class, new()
        {
            documentSession.Store(item);
            documentSession.SaveChanges();
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void Update<T>(T item) where T : class, new()
        {
            documentSession.Store(item);
            documentSession.SaveChanges();
        }
    }
}