using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
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
    }
}
