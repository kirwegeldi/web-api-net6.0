using HPlusSport.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;
        public ProductsController(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _context.Products.ToArrayAsync());
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
        #region Explaination of [FromBody] [FromRoute] [FromQuety]
            /*
                [FromBody] : Data from the body of the HTTP request , mostly used with HttpPost and HttpPut

                [FromRoute] : Data from the route template (for example enterng an productId to URL)

                [FromQuery] : Data from the URL 
             */
        #endregion
        
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            /*if(ModelState.IsValid)
            {
                return BadRequest();
            } */
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                "GetProduct",
                new {Id = product.Id},
                product);
        }

    }
}