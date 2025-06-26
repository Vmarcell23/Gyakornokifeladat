using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/shoppingitem")]
    [ApiController]
    public class ShoppingItemController : ControllerBase
    {
        private readonly IShoppingItemService _shoppingItemService;

        public ShoppingItemController(IShoppingItemService shoppingItemService)
        {
            _shoppingItemService = shoppingItemService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromBody] ShoppingItemCreateDto shoppingItemCreateDto)
        {
            try
            {
                var result = await _shoppingItemService.Create(shoppingItemCreateDto,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

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
        [Authorize]
        [HttpGet("items/my-family")]
        public async Task<IActionResult> GetAllItemsInFamily()
        {
            try
            {
                var result = await _shoppingItemService.GetAllInFamily(User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("item/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                var result = await _shoppingItemService.GetById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("vote")]
        public async Task<IActionResult> VoteItem(int itemId)
        {
            try
            {
                var result = await _shoppingItemService.Vote(itemId,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ShoppingItemCreateDto updateDto)
        {
            try
            {
                var result = await _shoppingItemService.Update(id, updateDto,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            try
            {
                var result = await _shoppingItemService.Delete(id,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
