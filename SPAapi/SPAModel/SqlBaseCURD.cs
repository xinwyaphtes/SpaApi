using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SPAModel
{
  public class SqlBaseCURD<T> : IDisposable where T : class
  {
    private DbContext db;

    public SqlBaseCURD()
    {
      db = new SPADEVContext();
    }

    public T Create(T value)
    {

      DbSet<T> dbSet = db.Set<T>();
      dbSet.Add(value);
      db.SaveChanges();
      return value;
    }

    public void Dispose()
    {
      db.Dispose();
    }

    public T Select(int id)
    {
      var dbSet = db.Set<T>().Find(id);
      return dbSet;
    }

  }
}
