using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_ValidationAndLogging.Data.Models;
using Task_ValidationAndLogging.Data;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Task_ValidationAndLogging.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Task_ValidationAndLogging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ApplicationDbContext context , ILogger<ProductsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        [HttpGet("getAll")]
        public  async Task<IActionResult> GetAllAsync()
        {
            var products = await context.Products.ToListAsync();
            var productsDto = products.Adapt<GetAllProductDtos>();

            return Ok(productsDto);
        }

        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            var productDto = product.Adapt<DetialsProductDtos>();
            return Ok(productDto);
        }

        [HttpPost("Create")]

        public async Task<IActionResult> CreateAsync(CreateProductDtos productDto , [FromServices] IValidator<CreateProductDtos> validator)
        {
            var validtion = validator.Validate(productDto);
            if (!validtion.IsValid)
            {
                var modelstate = new ModelStateDictionary();

                validtion.Errors.ForEach(error =>
                {
                    modelstate.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                );
                return ValidationProblem(modelstate);
            }
            var product = productDto.Adapt<Product>();
            await context.Products.AddAsync(product);
          await  context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("Update")]
        public  async Task<IActionResult> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            var Newproduct = dto.Adapt<Product>();

            await context.SaveChangesAsync();

            return Ok(Newproduct);
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product Not Found");
            }
           context.Products.Remove(product);
           await context.SaveChangesAsync();
            return Ok();
        }

    }
}
