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
using Microsoft.AspNetCore.Authentication;

namespace GyakorlatiFeladat.Services
{
    public interface IFamilyService
    {
        Task<List<FamilyDto>> GetAll();
        Task<FamilyDto> Create(FamilyCreateDto createDto,ClaimsPrincipal user);
        Task<FamilyDto> MyFamily(ClaimsPrincipal user);

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
            int existingFamilyId = await _context.FamilyUsers
                .Where(fu => fu.UserId == userId)
                .Select(fu => fu.FamilyId)
                .FirstOrDefaultAsync();
            if (existingFamilyId > 0)
                throw new InvalidOperationException("You are already a member of a family. You cannot create a new one.");

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

        public async Task<FamilyDto> MyFamily(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();
            int userId = int.Parse(userIdClaim.Value);
            var familyUser = await _context.FamilyUsers
                .Include(fu => fu.Family)
                .ThenInclude(f => f.FamilyUsers)
                .ThenInclude(fu => fu.User)
                .FirstOrDefaultAsync(fu => fu.UserId == userId);

            if (familyUser == null)
                throw new InvalidOperationException("You are not a member of any family.");
            return _mapper.Map<FamilyDto>(familyUser.Family);
        }


    }
}
