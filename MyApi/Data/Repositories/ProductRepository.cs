using System;
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
                           $"WHERE lower(\"Title\") LIKE '%{queryMetaDto.Search.ToLower()}%' ";

            if (queryMetaDto.Diagonals != "") sql += $" and \"Diagonal\" IN ({queryMetaDto.Diagonals}) ";
            if (queryMetaDto.Memories != "") sql += $" and \"Memory\" IN ({queryMetaDto.Memories}) ";
            if (queryMetaDto.Colors != "") sql += $" and \"Color\" IN ({queryMetaDto.Colors}) ";

            sql += $"ORDER BY \"{queryMetaDto.SortBy}\" {queryMetaDto.SortType} ;";

            IEnumerable<Product> addresses = await _context.Products.FromSqlRaw(sql).ToListAsync();
            
            return addresses.Skip(queryMetaDto.Offset * queryMetaDto.Limit).Take(queryMetaDto.Limit);
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
        
        public async Task Discount(QueryMetaDto queryMetaDto, double discount)
        {
            var disc = (100 - discount) / 100;
            var newPrice = discount == 0 ? "0" : $"ROUND(\"Price\" * {disc})";

            var sql = $"UPDATE \"Products\" "  +
                      $"SET \"NewPrice\" = {newPrice} " +
                      $"WHERE \"Title\" LIKE '%{queryMetaDto.Search}%' ";
            
            if (queryMetaDto.Diagonals != "") sql += $" and \"Diagonal\" IN ({queryMetaDto.Diagonals}) ";
            if (queryMetaDto.Memories != "") sql += $" and \"Memory\" IN ({queryMetaDto.Memories}) ";
            if (queryMetaDto.Colors != "") sql += $" and \"Color\" IN ({queryMetaDto.Colors}) ";
            
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}