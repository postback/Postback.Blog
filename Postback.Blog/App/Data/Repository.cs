using System;
using System.Linq;
using Postback.Blog.App.Repositories;

public class Repository<T> : IRepository<T>
{
    public T Save(T t)
    {
        throw new NotImplementedException();
    }

    public void Delete(T t)
    {
        throw new NotImplementedException();
    }

    public T Get(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> GetAll()
    {
        throw new NotImplementedException();
    }
}
