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
        
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutProduct(int Id, Product product)
        {
            if(Id != product.Id)
                return BadRequest();

            // product has been changed here    
            _context.Entry(product).State = EntityState.Modified;

            #region Why try catch used here?
                /* Try to save changes but what if this product has been 
                changed or deleted by any other asyc request?
                -> So catch statement checks whether if product is still in the context or if it has been changed.
                If it does not exist then it returns not-found*/
            #endregion
            try
            {
                await _context.SaveChangesAsync();      
            }
            catch
            {
                if(!_context.Products.Any(n => n.Id == Id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int Id)
        {
            var _product = await _context.Products.FindAsync(Id);
            if(_product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(_product);
            await _context.SaveChangesAsync();
            return _product;

        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult<List<Product>>> DeleteManyItem([FromQuery]int[] Ids)
        {
            var _products = await _context.Products.Where(n => Ids.Any(m => m == n.Id)).ToListAsync(); 
            if(Ids.Length > _products.Count)
            {
                return NotFound();
            }
            _context.RemoveRange(_products);
            await _context.SaveChangesAsync();
            return _products;
        }
    }
}