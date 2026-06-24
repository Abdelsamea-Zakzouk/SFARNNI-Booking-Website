using System.Linq;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo
{
    public class FlightProductRepository : GenericReposotory<FlightProduct>, IFlightProductRepository
    {
        private readonly AppDbContext _context;

        public FlightProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(FlightProduct flightProduct)
        {
            var flightProductInDb = _context.FlightProducts.FirstOrDefault(c => c.Id == flightProduct.Id);
            if (flightProductInDb != null)
            {
                flightProductInDb.Name = flightProduct.Name;
                flightProductInDb.Description = flightProduct.Description;
                flightProductInDb.LocationFrom = flightProduct.LocationFrom;
                flightProductInDb.LocationTo = flightProduct.LocationTo;
                flightProductInDb.TicketPrice = flightProduct.TicketPrice;
            }
        }
    }
}
