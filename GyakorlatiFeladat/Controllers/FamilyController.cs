using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService _familyService;

        public FamilyController(IFamilyService familyService)
        {
            _familyService = familyService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FamilyCreateDto createDto)
        {
            var result = await _familyService.Create(createDto, User);
            return Ok(result);
        }

        [HttpGet("families")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _familyService.GetAll();
            return Ok(result);
        }
        [Authorize]
        [HttpPost("invite")]
        public async Task<IActionResult> InviteToFamily(int invUserId)
        {
            try
            {
                var result = await _familyService.InviteToFamily(invUserId, User);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}