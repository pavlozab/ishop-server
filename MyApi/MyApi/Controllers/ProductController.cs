using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace MyApi.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : BaseController
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        /// <summary>
        /// Get Products.
        /// </summary>
        /// <response code="200">Returns Product List.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedResponseDto<ProductResponseDto>>> GetAll([FromQuery]QueryMetaDto queryMetaDto)
        {
            var count = await _service.Count();
            queryMetaDto.Validate();
            var addresses = await _service.GetAll(queryMetaDto);

            return Ok(new PaginatedResponseDto<ProductResponseDto>
            {
                Items = addresses,
                Meta = new MetaDto
                {
                    QueryMetaDto = queryMetaDto,
                    Count = count
                }
            });
        }

        /// <summary>
        /// Get a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved.</param>
        /// <response code="200">Returns a specific Product.</response>
        /// <response code="404">Product hasn't been found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductResponseDto>> GetOne(Guid id)
        {
            var product = await _service.GetOne(id);

            if (product is null)
                return NotFound("Product hasn't been found.");

            _logger .LogInformation("Returned product with id: {0}", id);
            return Ok(product);
        }

        /// <summary>
        /// Create a Product.
        /// </summary>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">Product is not created.</response>  
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateAddressDto addressDto)
        {
            var product = await _service.Create(addressDto);

            _logger.LogInformation("Create a Product");
            return CreatedAtAction(nameof(GetOne), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be updated.</param>
        /// <param name="productDto">Updated product.</param>
        /// <response code="204">Updated product.</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpPut("{id}")]
        //[Authorize(Roles = "ADMIN")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(Guid id, UpdateProductDto productDto)
        {
            try
            {
                await _service.Update(id, productDto);

                _logger.LogInformation("Updated product with id: {0}", id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id">The id of the item to be deleted</param>
        /// <response code="204">Deleted product</response>
        /// <response code="404">Product hasn't been found.</response>
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.Delete(id);

                _logger.LogInformation("Deleted product with id: {0}", id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("diagonals")]
        public async Task<ActionResult<IEnumerable<double>>> GetDiagonals()
        {
            var product = await _service.GetDiagonals();
            return Ok(product);
        }
        
        [AllowAnonymous]
        [HttpGet("colors")]
        public async Task<ActionResult<IEnumerable<string>>> GetColors()
        {
            var product = await _service.GetColors();
            return Ok(product);
        }
        
        [AllowAnonymous]
        [HttpGet("memories")]
        public async Task<ActionResult<IEnumerable<int>>> GetMemories()
        {
            var product = await _service.GetMemories();
            return Ok(product);
        }
    }
}