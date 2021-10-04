using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _context.Orders.Where(obj => obj.UserId == userId).ToListAsync();
        }
        
        public async Task<IEnumerable<Order>> GetCart(Guid userId)
        {
            return await _context.Orders.Where(obj => obj.UserId == userId && obj.IsCart).ToListAsync();
        }
        
        public async Task Buy(List<Guid> orders, Guid userId)
        {
            foreach (var orderId in orders)
            {
                var order = await _context.Orders.FirstAsync(obj => obj.UserId == userId && obj.Id == orderId);
                order.IsCart = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}