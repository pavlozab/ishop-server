using System;

namespace Entities
{
    public class ProductOrder
    {
        public Guid ProductOrderId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}