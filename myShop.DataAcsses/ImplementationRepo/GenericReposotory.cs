using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using myShop.DataAccess.Data;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class GenericReposotory<T> : IGenericRepostory<T> where T : class
{

    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;
    public GenericReposotory(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null , String? IncludeWord = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (IncludeWord != null){
            foreach (var item in IncludeWord.Split(new char[]{','} , StringSplitOptions.RemoveEmptyEntries ) ){
                query = query.Include(item);
            }
        }
        return query.ToList();
    }

    public T GetFirstorDefault(Expression<Func<T, bool>>? predicate = null , String? IncludeWord = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (IncludeWord != null){
            foreach (var item in IncludeWord.Split(new char[]{','} , StringSplitOptions.RemoveEmptyEntries ) ){
                query = query.Include(item);
            }
        }
#pragma warning disable CS8603 // Possible null reference return.
        return query.SingleOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
