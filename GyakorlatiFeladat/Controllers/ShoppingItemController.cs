using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingItemController : ControllerBase
    {
        private readonly IShoppingItemService _shoppingItemService;

        public ShoppingItemController(IShoppingItemService shoppingItemService)
        {
            _shoppingItemService = shoppingItemService;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var result = await _shoppingItemService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromBody] ShoppingItemCreateDto shoppingItemCreateDto)
        {
            try
            {
                var result = await _shoppingItemService.Create(shoppingItemCreateDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
