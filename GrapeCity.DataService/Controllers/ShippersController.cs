using GrapeCity.DataService.Models;
using Microsoft.AspNet.OData;
using System.Linq;

namespace GrapeCity.DataService.Controllers
{
    public class ShippersController : ODataController
    {
        private readonly NorthwindContext _context;

        public ShippersController(NorthwindContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<Shipper> Get()
        {
            return _context.Shippers;
        }

        [EnableQuery]
        public SingleResult<Shipper> Get([FromODataUri]int key)
        {
            return SingleResult.Create(_context.Shippers.Where(s => s.ShipperId == key));
        }

        [EnableQuery]
        public IQueryable<Order> GetOrders([FromODataUri] int key)
        {
            return _context.Shippers.Where(s => s.ShipperId == key).SelectMany(s => s.Orders);
        }
    }
}