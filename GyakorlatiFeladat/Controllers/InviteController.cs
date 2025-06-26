using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/invite")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _inviteService;

        public InviteController(IInviteService inviteService)
        {
            _inviteService = inviteService;
        }

        [Authorize]
        [HttpPost("invite")]
        public async Task<IActionResult> InviteToFamily(int invUserId)
        {
            try
            {
                var result = await _inviteService.InviteToFamily(invUserId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("accept-invite")]
        public async Task<IActionResult> AcceptInvite(bool accept, int familyId)
        {
            try
            {
                var result = await _inviteService.AcceptInvite(accept, familyId, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
