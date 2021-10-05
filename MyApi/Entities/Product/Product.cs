using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Product : BaseEntity
    {
        public string Image { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public int Memory { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }

        public int Amount { get; set; }
    }
    
    public class AddressConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Amount).IsRequired();
        }
    }
}