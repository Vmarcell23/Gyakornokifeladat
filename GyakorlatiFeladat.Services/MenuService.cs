using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace GyakorlatiFeladat.Services
{

    public interface IMenuService
    {
        public Task<MenuDto> Create(MenuCreateDto createDto, ClaimsPrincipal user);
        public Task<List<MenuDto>> GetFamilyMenus(ClaimsPrincipal user);
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

        public async Task<List<MenuDto>> GetFamilyMenus(ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var menus = await _context.Menus
                .Include(m => m.MenuRecipes)
                .ThenInclude(mr => mr.Recipe)
                .Where(m => m.FamilyId == familyId)
                .ToListAsync();

            return _mapper.Map<List<MenuDto>>(menus);
        }

        public async Task<MenuDto> Create(MenuCreateDto createDto, ClaimsPrincipal user)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));
           
            var familyId = _claimsHandler.GetFamilyId(user);

            var recipesInFamily = await _context.Recipes
                .Where(r => r.FamilyId == familyId && createDto.RecipeIds.Contains(r.Id))
                .ToListAsync();

            if(recipesInFamily.Count() != createDto.RecipeIds.Count())
                throw new InvalidOperationException("You can only create menus with recipes that belong to your family.");

            if (string.IsNullOrWhiteSpace(createDto.Name) || createDto.Name == "string" && createDto.RecipeIds.Count() > 0)
            {
                createDto.Name = string.Join(", ", recipesInFamily.Select(r => r.Name));
            }
            else
            {
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

    }
}
