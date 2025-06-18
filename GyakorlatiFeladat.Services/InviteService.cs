using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.Services
{
    public interface IInviteService
    {
        Task<FamilyInviteDto> InviteToFamily(int userId, ClaimsPrincipal user);
        Task<FamilyUserDto> AcceptInvite(bool accpet, int familyId, ClaimsPrincipal user);
    }

    public class InviteService : IInviteService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InviteService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FamilyInviteDto> InviteToFamily(int invUserId, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

           

            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (familyIdClaim == null || roleClaim == null || roleClaim.Value != "Owner")
                throw new UnauthorizedAccessException("You do not have permission to invite users");

            int userId = int.Parse(userIdClaim.Value);
            int familyId = int.Parse(familyIdClaim.Value);

            if (invUserId == userId)
                throw new InvalidOperationException("You cannot invite yourself to the family.");
            if (invUserId <= 0)
                throw new ArgumentException("Invalid user ID.");
            var existingInvite = await _context.FamilyInvites
                .FirstOrDefaultAsync(i => i.FamilyId == familyId && i.UserId == invUserId);
            if (existingInvite != null)
                throw new InvalidOperationException("This user has already been invited to the family.");
            var exisitingFamilyMember = await _context.FamilyUsers
                .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == invUserId);
            if (exisitingFamilyMember != null)
                throw new InvalidOperationException("This user is already a member of the family.");

            var invite = new FamilyInvite
            {
                FamilyId = familyId,
                UserId = invUserId,
                SentAt = DateTime.UtcNow,
                IsAccepted = false
            };

            _context.FamilyInvites.Add(invite);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyInviteDto>(invite);
        }

        public async Task<FamilyUserDto> AcceptInvite(bool accept, int familyId, ClaimsPrincipal user)
        {

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            int userId = int.Parse(userIdClaim.Value);

            var invite = await _context.FamilyInvites
                .FirstOrDefaultAsync(i => i.FamilyId == familyId && i.UserId == userId);
            if (invite == null)
                throw new InvalidOperationException("No invite found.");

            if (!accept)
            {
                _context.FamilyInvites.Remove(invite);
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("Invite declined.");
            }

            var familyUser = new FamilyUsers
            {
                FamilyId = familyId,
                UserId = userId,
                Role = Roles.FamilyMember
            };
            _context.FamilyUsers.Add(familyUser);

            _context.FamilyInvites.Remove(invite);

            await _context.SaveChangesAsync();

            return _mapper.Map<FamilyUserDto>(familyUser);
        }
    }
}
