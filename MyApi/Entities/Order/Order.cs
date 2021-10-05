using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Order: BaseEntity
    {
        public Guid UserId { get; set; }
        public List<ProductOrder> Products { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
    }
    
    
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
        }
    }
}