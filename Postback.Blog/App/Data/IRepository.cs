using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Postback.Blog.App.Repositories
{
    public interface IRepository<T>
    {
        T Save(T t);
        void Delete(T t);
        T Get(int id);
        IQueryable<T> GetAll();
    }
}
