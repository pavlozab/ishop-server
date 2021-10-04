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

        public async Task<OrderResponseDto> GetOne(Guid id, Guid userId)
        {
            var order = await _orderRepository.GetOne(id);
            
            if (order is null)
                throw new KeyNotFoundException("No order found.");

            if (order.Id != userId)
                throw new KeyNotFoundException("No order found.");

            var result = _mapper.Map<OrderResponseDto>(order);
            return result;
        }

        public async Task<OrderResponseDto> Create(CreateOrderDto newOrder, Guid userId)
        {
            var currentProduct = await _productsRepository.GetOne(newOrder.AddressId);
            
            if (currentProduct is null)
                throw new KeyNotFoundException("No Address found.");

            if (currentProduct.Amount < newOrder.Amount)
                throw new OutOfStockException("Out of stock", newOrder.Amount, currentProduct.Amount);

            var order = _mapper.Map<Order>(newOrder);
            order.UserId = userId;
            order.Date = DateTime.Now;
            
            currentProduct.Amount -= order.Amount;
            await _productsRepository.Update(currentProduct);
            await _orderRepository.Create(order);

            var result = _mapper.Map<OrderResponseDto>(order);
            return result;
        }
        
        public async Task Buy(List<Guid> orders, Guid userId)
        {
            await _orderRepository.Buy(orders, userId);
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