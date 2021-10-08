using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Entities;

namespace Data
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll(QueryMetaDto queryMetaDto); 
        Task Update(Product item);
        Task<long> Count();
        Task<IEnumerable<double>> GetDiagonals(); 
        Task<IEnumerable<string>> GetColors(); 
        Task<IEnumerable<int>> GetMemories();
        Task Discount(QueryMetaDto queryMetaDto, double discount);
    }
}