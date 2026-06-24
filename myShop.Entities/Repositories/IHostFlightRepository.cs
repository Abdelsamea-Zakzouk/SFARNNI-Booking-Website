using System;
using myShop.Entities.Models;
using myShop.Entities.ViewModels;

namespace myShop.Entities.Repositories;

public interface IHostFlightRepository : IGenericRepostory<HostFlight>
{
    int IncreaseCount(HostFlight hostFlight, int count);
    int DecreaseCount(HostFlight hostFlight, int count);
}
