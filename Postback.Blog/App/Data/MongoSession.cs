using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Norm;
using Norm.Collections;
using Norm.Linq;
using Norm.Responses;

namespace Postback.Blog.App.Data
{
    public class MongoSession : IDisposable, IPersistenceSession
    {
        private readonly string connectionString;

        public MongoSession()
        {
            connectionString = ConfigurationManager.AppSettings["mongo.db"];
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            var items = All<T>().Where(expression);
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.Database.GetCollection<T>().Delete(item);
            }
        }

        public void DeleteAll<T>() where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.Database.DropCollection(typeof(T).Name);
            }
        }

        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            T retval = default(T);
            using (var db = Mongo.Create(connectionString))
            {
                retval = db.GetCollection<T>().AsQueryable()
                           .Where(expression).SingleOrDefault();
            }
            return retval;
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                return db.GetCollection<T>().AsQueryable();
            }
        }

        public void Save<T>(T item) where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.GetCollection<T>().Save(item);
            }
        }

        public void Add<T>(T item) where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.GetCollection<T>().Insert(item);
            }
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.GetCollection<T>().Insert(items);
            }
        }

        public void Update<T>(T item) where T : class, new()
        {
            using (var db = Mongo.Create(connectionString))
            {
                db.GetCollection<T>().UpdateOne(item, item);
            }
        }

        public T MapReduce<T>(string map, string reduce)
        {
            T result = default(T);
            using (var db = Mongo.Create(connectionString))
            {
                var mr = db.Database.CreateMapReduce();
                MapReduceResponse response =
                    mr.Execute(new MapReduceOptions(typeof(T).Name)
                    {
                        Map = map,
                        Reduce = reduce
                    });
                var coll = response.GetCollection<MapReduceResult<T>>();
                MapReduceResult<T> r = coll.Find().FirstOrDefault();
                result = r.Value;
            }
            return result;
        }

        public void InsertIntoCollection(object item, string name)
        {

            using (var db = Mongo.Create(connectionString))
            {
                var collection = db.GetCollection<object>(name);
                collection.Insert(item);
            }
        }

        public void Dispose()
        {
            //_provider.Dispose();
        }
    }
}