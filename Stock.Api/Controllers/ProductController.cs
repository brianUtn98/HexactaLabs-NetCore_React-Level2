using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stock.Api.DTOs;
using Stock.Api.Extensions;
using Stock.AppService.Services;
using Stock.Model.Entities;

namespace Stock.Api.Controllers
{
    /// <summary>
    /// Product endpoint.
    /// </summary>
    [Produces("application/json")]
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ProductService service;
        private readonly IMapper mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="service">Product service.</param>
        /// <param name="mapper">Mapper configurator.</param>
        public ProductController(ProductService service,IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Adds a product.
        /// </summary>
        /// <param name="value">Product info.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] ProductDTO value)
        {
            TryValidateModel(value);

            try
            {
                var product = mapper.Map<Product>(value);
                service.Create(product);
                value.Id = product.Id;
                return Created(Url.Action("Get", new { Id = value.Id }), value);
            }
            catch
            {
                return BadRequest(new { Success = false, Message = "The name is already in use" });
            }
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            try
            {
                var result = service.GetAll();
                return mapper.Map<IEnumerable<ProductDTO>>(result).ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="id">Product id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<ProductDTO> Get(string id)
        {
            try
            {
                var result = service.Get(id);
                return mapper.Map<ProductDTO>(result);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">Product id.</param>
        /// <param name="value">Product information.</param>
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] ProductDTO value)
        {
            var product = service.Get(id);
            TryValidateModel(value);
            mapper.Map<ProductDTO, Product>(value, product);
            service.Update(product);
            return NoContent();
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">Product id to delete</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var product = service.Get(id);

                service.Delete(product);
                return Ok(new { Success = true, Message = "", data = id });
            }
            catch
            {
                return Ok(new { Success = false, Message = "", data = id });
            }
        }

        /// <summary>
        /// Search some products.
        /// </summary>
        /// <param name="model">Product filters.</param>
        /// <returns></returns>
        [HttpPost("search")]
        public ActionResult Search([FromBody] ProductSearchDTO model)
        {
            Expression<Func<Product, bool>> filter = x => !string.IsNullOrWhiteSpace(x.Id);

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                filter = filter.AndOrCustom(
                    x => x.Name.ToUpper().Contains(model.Name.ToUpper()),
                    model.Condition.Equals(ActionDto.AND));
            }

            if (!string.IsNullOrWhiteSpace(model.CostPrice))
            {
                filter = filter.AndOrCustom(
                    x => x.CostPrice.ToString().ToUpper().Contains(model.CostPrice.ToUpper()),
                    model.Condition.Equals(ActionDto.AND));
            }

            if(!string.IsNullOrWhiteSpace(model.SalePrice))
            {
                filter = filter.AndOrCustom(
                    x => x.SalePrice.ToString().ToUpper().Contains(model.SalePrice.ToUpper()),
                    model.Condition.Equals(ActionDto.AND));
            }

            var products = service.Search(filter);
            return Ok(products);
        }
    }
}