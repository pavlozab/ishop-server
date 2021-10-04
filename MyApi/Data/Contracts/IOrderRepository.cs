using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Data
{
    public interface IOrderRepository: IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
        Task Buy(List<Guid> orders, Guid userId);
    }
}