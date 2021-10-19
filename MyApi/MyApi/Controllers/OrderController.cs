using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace MyApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController: BaseController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ApplicationDbContext context)
        {
            _logger = logger;
            _orderService = orderService;
            _context = context;
        }

        /// <summary>
        /// Get all your Orders.
        /// </summary>
        /// <returns>All your orders.</returns>
        /// <response code="200">Orders are successfully returned.</response>
        /// <response code="401">Invalid token.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult GetAllOrders()
        {
            try
            {
                var userId = GetCurrentUserId();

                _logger.LogInformation("Orders is successfully returned");

                var response = _context.Orders.Where(obj => obj.UserId == userId)
                    .Select(order => new
                    {
                        order.Date,
                        Products = order.Products.Select(ob => new
                        {
                            Title = _context.Products.FirstOrDefault(prod => prod.Id == ob.ProductId).Title,
                            ob.Amount,
                            _context.Products.FirstOrDefault(prod => prod.Id == ob.ProductId).NewPrice,
                            _context.Products.FirstOrDefault(prod => prod.Id == ob.ProductId).Price
                        }),
                        order.TotalPrice
                    });
                return Ok(response);
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Get a specific Order.
        /// </summary>
        /// <param name="id">The id of the order to be retrieved.</param>
        /// <returns>Specific Order.</returns>
        /// <response code="200">Returns order.</response>
        /// <response code="401">Invalid token.</response>
        /// <response code="404">Order hasn't been found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();

                _logger.LogInformation("Order is successfully returned");
                return Ok(await _orderService.GetOne(id, userId));
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        /// <summary>
        /// Create a new Order.
        /// </summary>
        /// <param name="newOrder">New Order</param>
        /// <returns>New Order</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="403">Not enough product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreateOrder(Order newOrder)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _orderService.Create(newOrder, userId);

                return StatusCode(201, new
                {
                    order.Date,
                    order.Products,
                    order.TotalPrice
                });
            }
            catch (SecurityTokenValidationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (OutOfStockException e)
            {
                return BadRequest($"Out of stock. Your amount is {e.OrderAmount}, but we have only {e.ProductAmount}");
            }
        }
    }
}