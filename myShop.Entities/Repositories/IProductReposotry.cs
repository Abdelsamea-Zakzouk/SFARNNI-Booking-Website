

using System;
using myShop.Entities.Models;

namespace myShop.Entities.Repositories;

public interface IProductReposotry : IGenericRepostory<Product>
{
    void Update(Product product);
}