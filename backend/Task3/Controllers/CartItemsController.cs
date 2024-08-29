using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    public class CartItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly MyDbContext _db;

        public CartItemsController(MyDbContext db)
        {
            _db = db;
        }

        // Get all Items

        // Get all Cart Items
        [HttpGet]
        [Route("AllItems")]
        public IActionResult GetAllCartItems()
        {
            var CartItems = _db.CartItems.Select(x =>
            
            new CartItemsResponsive
            {
                CartId = x.CartId,
                //CartItemId = x.CartItemId,
                Quantity = x.Quantity,
                Product = new ProductDto
                {
                    ProductName = x.Product.ProductName, 
                    Price = x.Product.Price ,
                    Image=x.Product.Image
                }
            });

            return Ok(CartItems);
        }


        [HttpPost]
        [Route("AddItem")]
        public IActionResult AddCartItem([FromBody] AddCartItemsDto newItem)
        {
            

            // Create a new cart item
            var cartItem = new CartItem
            {
                CartId = newItem.CartId,
                ProductId = newItem.ProductId,
                Quantity = newItem.Quantity
            };

            // Add the cart item to the database
            _db.CartItems.Add(cartItem);
            _db.SaveChanges();

            return Ok(new { Message = "Item added to cart successfully.", CartItem = cartItem });
        }
    }

}
