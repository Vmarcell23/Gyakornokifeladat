using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GyakorlatiFeladat.Services
{
    public interface IRecipeService
    {
        public Task<RecipeDto> Create(RecipeCreateDto createDto, ClaimsPrincipal user);
        public Task<List<RecipeDto>> GetAll();
        public Task<List<RecipeDto>> GetFamilyRecipes(ClaimsPrincipal user);
        public Task<RecipeDto> Update(int id, RecipeCreateDto updateDto, ClaimsPrincipal user);
        public Task<RecipeDto> Delete(int id, ClaimsPrincipal user);

    }
    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsHandler _claimsHandler;
        public RecipeService(AppDbContext context, IMapper mapper, IClaimsHandler claimsHandler)
        {
            _context = context;
            _mapper = mapper;
            _claimsHandler = claimsHandler;
        }

        public async Task<RecipeDto> Create(RecipeCreateDto createDto, ClaimsPrincipal user)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));
            if (string.IsNullOrWhiteSpace(createDto.Name) || createDto.Name == "string")
                throw new ArgumentException("Recipe name is required");

            var familyId = _claimsHandler.GetFamilyId(user);
            var userId = _claimsHandler.GetUserId(user);
            var recipe = _mapper.Map<Recipe>(createDto);
            recipe.CreatorId = userId;
            recipe.FamilyId = familyId;

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<List<RecipeDto>> GetAll()
        {
            var recipes = await _context.Recipes.ToListAsync();
            return _mapper.Map<List<RecipeDto>>(recipes);
        }

        public async Task<List<RecipeDto>> GetFamilyRecipes(ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var recipes = await _context.Recipes
                .Where(r => r.FamilyId == familyId)
                .ToListAsync();
            return _mapper.Map<List<RecipeDto>>(recipes);
        }
        
        public async Task<RecipeDto> Update(int id, RecipeCreateDto updateDto, ClaimsPrincipal user)
        {
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));
            if (string.IsNullOrWhiteSpace(updateDto.Name) || updateDto.Name == "string")
                throw new ArgumentException("Recipe name is required");

            var familyId = _claimsHandler.GetFamilyId(user);
            var recipe = findbyid(id);
            var userId = _claimsHandler.GetUserId(user);
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Admin && userRole != Roles.Owner && userId != recipe.CreatorId && recipe.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to update this recipe.");
            
            recipe = _mapper.Map(updateDto, recipe);
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();

            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<RecipeDto> Delete(int id, ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var recipe = findbyid(id);
            if (recipe.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to delete this recipe.");

            var userId = _claimsHandler.GetUserId(user);
            var userRole = _claimsHandler.GetUserRole(user);
            if (userRole != Roles.Admin && userRole != Roles.Owner && userId != recipe.CreatorId) 
                throw new UnauthorizedAccessException("You do not have permission to delete this recipe.");

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return _mapper.Map<RecipeDto>(recipe);
        }
        
        private Recipe findbyid(int id)
        {
            var recipe = _context.Recipes
                .Include(r => r.family)
                .FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            return recipe;
        }
    }
}
