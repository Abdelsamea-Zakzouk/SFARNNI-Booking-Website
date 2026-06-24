using System;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class ApplicationUsersRepostory : GenericReposotory<ApplicationUsers>, IApplicationUsersRepostory
{
    private readonly AppDbContext _context;
    public ApplicationUsersRepostory(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
