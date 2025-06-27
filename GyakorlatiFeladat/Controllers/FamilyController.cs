using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/family")]
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
            try
            {
                var result = await _familyService.Create(createDto, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("families")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _familyService.GetAll();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("my-family")]
        public async Task<IActionResult> MyFamily()
        {
            try
            {
                var result = await _familyService.MyFamily(User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("family-details/{familyId}")]
        public async Task<IActionResult> GetFamilyDetailsById(int familyId)
        {
            try
            {
                var result = await _familyService.GetFamilyDetailsById(familyId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("add-admin")]
        public async Task<IActionResult> AddAdmin(int userId)
        {
            try
            {
                var result = await _familyService.AddAdmin(userId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("remove-admin")]
        public async Task<IActionResult> RemoveAdmin(int userId)
        {
            try
            {
                var result = await _familyService.RemoveAdmin(userId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("changename/{name}")]
        public async Task<IActionResult> UpdateName(string name)
        {
            try
            {
                var result = await _familyService.UpdateName(name, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("remove-member")]
        public async Task<IActionResult> RemoveMember(int userId)
        {
            try
            {
                var result = await _familyService.RemoveMember(userId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int familyId)
        {
            try
            {
                var result = await _familyService.Delete(familyId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}