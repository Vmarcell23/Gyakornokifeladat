using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GyakorlatiFeladat.DataContext.Entities;

namespace GyakorlatiFeladat.Services
{
    public interface IClaimsHandler
    {
        int GetUserId(ClaimsPrincipal user);
        int GetFamilyId(ClaimsPrincipal user);
        Roles GetUserRole(ClaimsPrincipal user);
    }

    public class ClaimsHandler : IClaimsHandler
    {
        public int GetUserId(ClaimsPrincipal user)
        {
            var userClaimId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userClaimId == null)
                throw new UnauthorizedAccessException("User is not authenticated.");
            var userId = int.Parse(userClaimId.Value);
            return userId;
        }

        public int GetFamilyId(ClaimsPrincipal user)
        {
            var familyClaimId = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            if (familyClaimId == null)
                throw new UnauthorizedAccessException("User is not a member of any family.");
            var familyId = int.Parse(familyClaimId.Value);
            return familyId;
        }
        
        public Roles GetUserRole(ClaimsPrincipal user)
        {
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim == null)
                throw new UnauthorizedAccessException("User does not have a role assigned.");
            if (!Enum.TryParse(roleClaim.Value, out Roles role))
                throw new InvalidOperationException("Invalid role assigned to user.");
            return role;
        }
    }
}
