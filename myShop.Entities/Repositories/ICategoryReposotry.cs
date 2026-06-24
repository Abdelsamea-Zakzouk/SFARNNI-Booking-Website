using System;
using myShop.Entities.Models;

namespace myShop.Entities.Repositories;

public interface ICategoryReposotry : IGenericRepostory<Category>
{
    void Update(Category category);
}
