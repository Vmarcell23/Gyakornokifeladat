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

        [HttpPut("vote")]
        public async Task<IActionResult> VoteItem([FromBody] ShoppingItemCreateVoteDto voteDto)
        {
            try
            {
                var result = await _shoppingItemService.Vote(voteDto);
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
                var result = await _shoppingItemService.Update(id, updateDto);
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
                var result = await _shoppingItemService.Delete(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
