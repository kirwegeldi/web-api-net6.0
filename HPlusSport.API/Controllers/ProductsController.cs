using HPlusSport.API.Models;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("all-products")]
        public ActionResult GetAllProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
        [HttpGet("{Id}")]
        public ActionResult GetProductById(int Id)
        {
            var product = _context.Products.Find(Id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}