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
using Microsoft.VisualBasic;

namespace GyakorlatiFeladat.Services
{
    public interface IFamilyService
    {
        Task<List<FamilyDto>> GetAll();
        Task<FamilyDto> Create(FamilyCreateDto createDto,ClaimsPrincipal user);
        Task<FamilyDto> MyFamily(ClaimsPrincipal user);
        Task<FamilyDeatliedDto> GetFamilyDetailsById(int familyId);
        Task<FamilyUserDto> AddAdmin(int userId, ClaimsPrincipal user);
        Task<FamilyUserDto> RemoveAdmin(int userId, ClaimsPrincipal user);
        Task<FamilyUserDto> RemoveMember(int userId, ClaimsPrincipal user);
        Task<FamilyDto> UpdateName(string name, ClaimsPrincipal user);
        Task<FamilyDto> Delete(ClaimsPrincipal user);

    }
    public class FamilyService : IFamilyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsHandler _claimsHandler;
        public FamilyService(AppDbContext context, IMapper mapper, IClaimsHandler claimsHandler)
        {
            _context = context;
            _mapper = mapper;
            _claimsHandler = claimsHandler;
        }
        public async Task<FamilyDto> Create(FamilyCreateDto createDto, ClaimsPrincipal user)
        {

            var userId = _claimsHandler.GetUserId(user);
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

        public async Task<FamilyDeatliedDto> GetFamilyDetailsById(int familyId)
        {
            var family = await _context.Families
                .Include(f => f.FamilyUsers)
                .ThenInclude(fu => fu.User)
                .Include(f => f.TaskItems)
                .Include(f => f.ShoppingItems)
                .ThenInclude(si => si.Votes)
                .Include(f => f.Recipes)
                .Include(f => f.Menus)
                .ThenInclude(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .Include(f => f.Menus)
                .ThenInclude(m => m.Votes)
                .FirstOrDefaultAsync(f => f.Id == familyId);
            if (family == null)
                throw new KeyNotFoundException("Family not found.");
            return _mapper.Map<FamilyDeatliedDto>(family);
        }

        public async Task<FamilyDto> MyFamily(ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var myfamily = await _context.Families
               .Include(f => f.FamilyUsers)
               .ThenInclude(f => f.User)
               .Where(f => f.Id == familyId)
               .FirstOrDefaultAsync();

            return _mapper.Map<FamilyDto>(myfamily);
        }

        public async Task<FamilyUserDto> AddAdmin(int userId, ClaimsPrincipal user)
        {
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Owner && userRole != Roles.Admin)
                throw new UnauthorizedAccessException("Only family owner or admins can promote users to admin.");
            
            var familyId = _claimsHandler.GetFamilyId(user);
            var familyUser = await _context.FamilyUsers
                .Include(fu => fu.User)
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.FamilyId == familyId);
            if (familyUser == null)
                throw new KeyNotFoundException("User not found in the family.");
            if (familyUser.Role == Roles.Owner)
                throw new InvalidOperationException("You cannot promote an owner to admin.");

            familyUser.Role = Roles.Admin;
            _context.Update(familyUser);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyUserDto>(familyUser);
        }

        public async Task<FamilyUserDto> RemoveAdmin(int userId, ClaimsPrincipal user)
        {
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Owner)
                throw new UnauthorizedAccessException("Only family owner can remove admin rights.");
            var familyId = _claimsHandler.GetFamilyId(user);
            var familyUser = await _context.FamilyUsers
                .Include(fu => fu.User)
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.FamilyId == familyId);
            if (familyUser == null)
                throw new KeyNotFoundException("User not found in the family.");
            if (familyUser.Role != Roles.Admin)
                throw new InvalidOperationException("The user is not Admin");

            familyUser.Role = Roles.FamilyMember;
            _context.Update(familyUser);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyUserDto>(familyUser);
        }

        public async Task<FamilyUserDto> RemoveMember(int userId, ClaimsPrincipal user)
        {
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Admin && userRole != Roles.Owner)
                throw new UnauthorizedAccessException("Only family owner or admins can remove users from the family.");
            
            var familyId = _claimsHandler.GetFamilyId(user);
            var familyUser = await _context.FamilyUsers
                .Include(fu => fu.User)
                .FirstOrDefaultAsync(fu => fu.UserId == userId && fu.FamilyId == familyId);
            if (familyUser == null)
                throw new KeyNotFoundException("User not found in the family.");
            if (familyUser.Role == Roles.Admin && userRole != Roles.Owner)
                throw new UnauthorizedAccessException("You cannot remove an admin unless you are the owner.");
            if (familyUser.Role == Roles.Owner)
                throw new InvalidOperationException("You cannot remove the owner.");

            _context.FamilyUsers.Remove(familyUser);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyUserDto>(familyUser);
        }

        public async Task<FamilyDto> UpdateName(string name, ClaimsPrincipal user)
        {
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Owner)
                throw new UnauthorizedAccessException("Only family owner or admins can update the family name.");

            var familyId = _claimsHandler.GetFamilyId(user);
            var family = _context.Families
                .FirstOrDefault(f => f.Id == familyId);
            if (family == null)
                throw new KeyNotFoundException("Family not found or you are not a member of this family.");

            if (string.IsNullOrWhiteSpace(name) && name == "string")
                throw new ArgumentException("Family name is required");
            
            family.Name = name;
            _context.Families.Update(family);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyDto>(family);
        }

        public async Task<FamilyDto> Delete(ClaimsPrincipal user)
        {
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Owner)
                throw new UnauthorizedAccessException("Only family owner can delete the family.");
            var familyId = _claimsHandler.GetFamilyId(user);

            var family = await _context.Families
                .Include(f => f.FamilyUsers)
                .FirstOrDefaultAsync(f => f.Id == familyId);

            var users = _context.Users
                .Include(u => u.FamilyUsers)
                .FirstOrDefaultAsync(f => f.Id == familyId);

            
            if (family == null)
                throw new KeyNotFoundException("Family not found.");
            
            
            _context.Families.Remove(family);
            await _context.SaveChangesAsync();
            return _mapper.Map<FamilyDto>(family);
        }
    }
}
