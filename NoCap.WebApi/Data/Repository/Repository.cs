using Microsoft.EntityFrameworkCore;

namespace NoCap.Data.Repository
{
    public class Repository<T> where T : class
    {
        private readonly IdentityContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(IdentityContext dbContext, DbSet<T> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }
        public virtual void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
        }
        public virtual T GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return _dbSet.Find(id);
        }
        public virtual IEnumerable<T> GetAll(T entity) 
        {
            return _dbSet.ToList();
        }
        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var entity = GetById(id);
            _dbSet.Remove(entity);
        }
    }
}
