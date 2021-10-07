using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAll(QueryMetaDto queryMetaDto)
        {
            var sql = $"SELECT * FROM \"Products\" " +
                           $"WHERE \"Title\" LIKE '%{queryMetaDto.Search}%' ";
            
            if (queryMetaDto.Diagonals != "") sql += $" and \"Diagonal\" IN ({queryMetaDto.Diagonals}) ";
            if (queryMetaDto.Memories != "") sql += $" and \"Memory\" IN ({queryMetaDto.Memories}) ";
            if (queryMetaDto.Colors != "") sql += $" and \"Color\" IN ({queryMetaDto.Colors}) ";

            sql += $"ORDER BY \"{queryMetaDto.SortBy}\" {queryMetaDto.SortType} " + 
                   $"LIMIT {queryMetaDto.Limit} OFFSET {queryMetaDto.Offset};";
            
            IEnumerable<Product> addresses = await _context.Products.FromSqlRaw(sql).ToListAsync();
            
            return addresses;
        }
        
        public async Task Update(Product obj)
        {
            _context.Products.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Count()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<IEnumerable<double>> GetDiagonals()
        {
            return await _context.Products.Select(obj => obj.Diagonal).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetColors()
        {
            return await _context.Products.Select(obj => obj.Color).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<int>> GetMemories()
        {
            return await _context.Products.Select(obj => obj.Memory).Distinct().ToListAsync();
        }
    }
}