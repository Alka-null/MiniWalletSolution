    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using global::DataAccess.Interfaces;

    namespace DataAccess
    {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }

}
