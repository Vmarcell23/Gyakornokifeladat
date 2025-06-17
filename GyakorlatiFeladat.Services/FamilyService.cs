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
    public interface IFamilyService
    {
        Task<List<FamilyDto>> GetAll();
        Task<FamilyDto> Create(FamilyCreateDto createDto,ClaimsPrincipal user);
        Task<FamilyInviteDto> InviteToFamily(int userId,ClaimsPrincipal user);
    }
    public class FamilyService : IFamilyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public FamilyService (AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FamilyDto> Create(FamilyCreateDto createDto, ClaimsPrincipal user)
        {

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            int userId = int.Parse(userIdClaim.Value);


            var family = _mapper.Map<Family>(createDto);
            family.FamilyUsers = new List<FamilyUsers>
            {
                new FamilyUsers
                {
                    UserId = userId,
                    Role = Roles.Owner
                }
            };
            await _context.Families.AddAsync(family);
            await _context.SaveChangesAsync();

            return _mapper.Map<FamilyDto>(family);  
        }

        public async Task<List<FamilyDto>> GetAll()
        {
            var families = await _context.Families
                .Include(f => f.FamilyUsers)
                .ThenInclude(fu => fu.User)
                .ToListAsync();

            return _mapper.Map<List<FamilyDto>>(families);
        }

        public async Task<FamilyInviteDto> InviteToFamily(int invUserId, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            int userId = int.Parse(userIdClaim.Value);

            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (familyIdClaim == null || roleClaim == null || roleClaim.Value != "Owner")
                throw new UnauthorizedAccessException("You do not have permission to invite users");

            int familyId = int.Parse(familyIdClaim.Value);
            
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
    }
}
