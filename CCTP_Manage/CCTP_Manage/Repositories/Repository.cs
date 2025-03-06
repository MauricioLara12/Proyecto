using CCTP_Manage.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CCTP_Manage.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly SaloneventosdbContext context;

        public Repository(SaloneventosdbContext context)
        {
            this.context = context;
        }

        public void Insert(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

        public T? Get(object id)
        {
            return context.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

    }

}
