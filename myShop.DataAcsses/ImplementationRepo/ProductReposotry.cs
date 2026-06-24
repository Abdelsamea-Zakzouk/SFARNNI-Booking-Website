using System;
using System.Security.Cryptography.X509Certificates;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class ProductReposotry : GenericReposotory<Product>, IProductReposotry
{
    private readonly AppDbContext _context;
    public ProductReposotry(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Product product)
    {
        var ProductInDb = _context.Products.FirstOrDefault(c => c.Id == product.Id);
        if (ProductInDb != null)
        {
            ProductInDb.Name = product.Name;
            ProductInDb.Description = product.Description;
            ProductInDb.PricePerNight = product.PricePerNight;
            ProductInDb.Image = product.Image;
            ProductInDb.Category = product.Category;
            ProductInDb.Location = product.Location;
        }
    }
}
