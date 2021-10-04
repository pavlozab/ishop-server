using System;

namespace Dto
{
    public class ProductResponseDto //: CreateOrderDto  , BaseDto<,>
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public int Memory { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}