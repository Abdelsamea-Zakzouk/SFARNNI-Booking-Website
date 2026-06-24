using System;
using System.Security.Cryptography.X509Certificates;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class CategoryReposotry : GenericReposotory<Category>, ICategoryReposotry
{
    private readonly AppDbContext _context;
    public CategoryReposotry(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Category category)
    {
        var CategoryInDb = _context.Categorys.FirstOrDefault(c => c.Id == category.Id);
        if (CategoryInDb != null)
        {
            CategoryInDb.Name = category.Name;
            CategoryInDb.Description = category.Description;
            CategoryInDb.Date = DateTime.Now;
        }
    }
}
