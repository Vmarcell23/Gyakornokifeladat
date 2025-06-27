using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateMenu([FromBody] MenuCreateDto createCreateDto)
        {
            try
            {
                var result = await _menuService.Create(createCreateDto, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("vote/{id}")]
        public async Task<IActionResult> VoteMenu(int id)
        {
            try
            {
                var result = await _menuService.Vote(id, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("menus")]
        public async Task<IActionResult> GetAllMenus()
        {
            try
            {
                var menus = await _menuService.GetAll();
                return Ok(menus);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("family-menus")]
        public async Task<IActionResult> GetFamilyMenus()
        {
            try
            {
                var menus = await _menuService.GetFamilyMenus(User);
                return Ok(menus);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuCreateDto updateDto)
        {
            try
            {
                var result = await _menuService.Update(id, updateDto, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var result = await _menuService.Delete(id, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
