using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Entities;
using Dto;

namespace Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productsRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productsRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productsRepository = productsRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<Order>> GetAll(Guid userId)
        {
            return await _orderRepository.GetAll(userId);
        }

        public async Task<Order> GetOne(Guid id, Guid userId)
        {
            var order = await _orderRepository.GetOne(id);
            
            if (order is null)
                throw new KeyNotFoundException("No order found.");

            if (order.Id != userId)
                throw new KeyNotFoundException("No order found.");
            
            return order;
        }

        public async Task<Order> Create(Order newOrder, Guid userId)
        {
            var orderId = Guid.NewGuid();
            newOrder.Id = orderId;

            foreach (var product in newOrder.Products)
            {
                var currentProduct = await _productsRepository.GetOne(product.ProductId);
                if (currentProduct is null)
                    throw new KeyNotFoundException("No Product found.");
                
                if (currentProduct.Amount < product.Amount)
                    throw new OutOfStockException("Out of stock", product.Amount, currentProduct.Amount);
                
                product.OrderId = orderId;
                product.ProductOrderId = Guid.NewGuid();
                currentProduct.Amount -= product.Amount;
                await _productsRepository.Update(currentProduct);

                newOrder.TotalPrice += currentProduct.NewPrice > 0
                    ? product.Amount * currentProduct.NewPrice
                    : product.Amount * currentProduct.Price;
            }
            
            newOrder.UserId = userId;
            newOrder.Date = DateTime.Now;
            
            await _orderRepository.Create(newOrder);
            return newOrder;
        }
    }

    public class OutOfStockException : ApplicationException
    {
        public decimal OrderAmount { get; set; }
        public decimal ProductAmount { get; set; }

        public OutOfStockException(string message, decimal orderAmount, decimal productAmount) 
            : base(message)
        {
            OrderAmount = orderAmount;
            ProductAmount = productAmount;
        }
    }
}
