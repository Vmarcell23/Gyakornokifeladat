using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace GyakorlatiFeladat.Services
{

    public interface IMenuService
    {
        public Task<MenuDto> Create(MenuCreateDto createDto, ClaimsPrincipal user);
        public Task<List<MenuDto>> GetFamilyMenus(ClaimsPrincipal user);
        public Task<List<MenuDto>> GetAll();
        public Task<MenuDto> Update(int id, MenuCreateDto updateDto, ClaimsPrincipal user);
        public Task<MenuDto> Delete(int id, ClaimsPrincipal user);
        public Task<MenuDto> Vote(int id, ClaimsPrincipal user);
    }

    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsHandler _claimsHandler;

        public MenuService(AppDbContext context, IMapper mapper, IClaimsHandler claimsHandler)
        {
            _context = context;
            _mapper = mapper;
            _claimsHandler = claimsHandler;
        }

        public async Task<MenuDto> Create(MenuCreateDto createDto, ClaimsPrincipal user)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            var familyId = _claimsHandler.GetFamilyId(user);

            var recipesInFamily = await _context.Recipes
                .Where(r => r.FamilyId == familyId && createDto.RecipeIds.Contains(r.Id))
                .ToListAsync();

            if (recipesInFamily.Count() != createDto.RecipeIds.Count())
                throw new InvalidOperationException(
                    "You can only create menus with recipes that belong to your family.");

            if (string.IsNullOrWhiteSpace(createDto.Name) || createDto.Name == "string")
            {
                if (createDto.RecipeIds.Count() > 0)
                    createDto.Name = string.Join(", ", recipesInFamily.Select(r => r.Name));
                else
                    throw new ArgumentException("Name is required");
            }

            var userId = _claimsHandler.GetUserId(user);
            var menu = _mapper.Map<Menu>(createDto);
            menu.FamilyId = familyId;
            menu.CreatorId = userId;

            menu.MenuRecipes = recipesInFamily
                .Select(r => new MenuRecipe
                {
                    RecipeId = r.Id,
                    MenuId = menu.Id
                }).ToList();

            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task<List<MenuDto>> GetAll()
        {
            var menus = await _context.Menus
                .Include(m => m.Votes)
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .ToListAsync();
            return _mapper.Map<List<MenuDto>>(menus);
        }

        public async Task<List<MenuDto>> GetFamilyMenus(ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var menus = await _context.Menus
                .Include(m => m.Votes)
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .Where(m => m.FamilyId == familyId)
                .ToListAsync();

            return _mapper.Map<List<MenuDto>>(menus);
        }

        public async Task<MenuDto> Vote(int id, ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
       

            var menu = await _context.Menus
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .Include(m => m.Votes)
                .FirstOrDefaultAsync(m => m.Id == id && m.FamilyId == familyId);
            if (menu == null)
                throw new KeyNotFoundException("Menu is not found in the family.");

            var userId = _claimsHandler.GetUserId(user);
            if (menu.Votes.Any(v => v.UserId == userId))
                throw new InvalidOperationException("You have already voted for this menu.");

            var vote = new MenuVote
            {
                MenuId = menu.Id,
                UserId = userId
            };
            var familyMemberCount = await _context.FamilyUsers //Ha mindenki zavasz akkor az IsNeeded true lesz
                .Where(fu => fu.FamilyId == familyId)
                .CountAsync();
            menu.Votes.Add(vote);
            if (menu.Votes.Count() == familyMemberCount)
                menu.isNeeded = true;

            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task<MenuDto> Update(int id, MenuCreateDto updateDto, ClaimsPrincipal user)
        {
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            var menu = await _context.Menus
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);

            var familyId = _claimsHandler.GetFamilyId(user);
            var userId = _claimsHandler.GetUserId(user);
            var userRole = _claimsHandler.GetUserRole(user);

            if (userRole != Roles.Owner && userRole != Roles.Admin && userId != menu.CreatorId)
                throw new UnauthorizedAccessException("You dont have permission to update the menu!");
            var recipesInFamily = await _context.Recipes
                .Where(r => r.FamilyId == familyId && updateDto.RecipeIds.Contains(r.Id))
                .ToListAsync();
            
            if (recipesInFamily.Count() != updateDto.RecipeIds.Count())
                throw new InvalidOperationException(
                    "You can only create menus with recipes that belong to your family.");

            if (string.IsNullOrWhiteSpace(updateDto.Name) || updateDto.Name == "string")
            {
                if (updateDto.RecipeIds.Count() > 0)
                    updateDto.Name = string.Join(", ", recipesInFamily.Select(r => r.Name));
                else
                    throw new ArgumentException("Name is required");
            }

            menu = _mapper.Map(updateDto, menu);

            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task<MenuDto> Delete(int id, ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var menu = await _context.Menus
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id && m.FamilyId == familyId);
            if (menu == null)
                throw new KeyNotFoundException("Menu is not found in the family.");
            var userRole = _claimsHandler.GetUserRole(user);
            var userId = _claimsHandler.GetUserId(user);
            if (userRole != Roles.Owner && userRole != Roles.Admin && userId != menu.CreatorId)
                throw new UnauthorizedAccessException("You do not have permission to delete this menu.");

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return _mapper.Map<MenuDto>(menu);
        }


    }

}
