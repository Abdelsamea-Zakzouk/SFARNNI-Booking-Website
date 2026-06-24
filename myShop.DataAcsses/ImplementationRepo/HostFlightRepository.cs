using System;
using System.Security.Cryptography.X509Certificates;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;

namespace myShop.DataAccess.ImplementationRepo;

public class HostFlightRepository : GenericReposotory<HostFlight>, IHostFlightRepository
{
    private readonly AppDbContext _context;
    public HostFlightRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public int DecreaseCount(HostFlight hostFlight, int count)
    {
        hostFlight.Count -= count;
        return hostFlight.Count;
    }

    public int IncreaseCount(HostFlight hostFlight, int count)
    {
        hostFlight.Count += count;
        return hostFlight.Count;
    }
}
