using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.Services
{
    public interface IShoppingItemService
    {
        Task<List<ShoppingItemDto>> GetAll();
        Task<List<ShoppingItemDto>> GetAllInFamily(ClaimsPrincipal user);
        Task<ShoppingItemDto> GetById(int id);
        Task<ShoppingItemDto> Create(ShoppingItemCreateDto shoppingItemCreateDto,ClaimsPrincipal user);
        Task<ShoppingItemDto> Vote(int itemId, ClaimsPrincipal user);
        Task<ShoppingItemDto> Update(int id, ShoppingItemCreateDto updateDto, ClaimsPrincipal user);
        Task<ShoppingItemDto> Delete(int id, ClaimsPrincipal user);
     

    }
    public class ShoppingItemService : IShoppingItemService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsHandler _claimsHandler;
        public ShoppingItemService(AppDbContext context, IMapper mapper, IClaimsHandler claimsHandler)
        {
            _context = context;
            _mapper = mapper;
            _claimsHandler = claimsHandler;
        }
        public async Task<ShoppingItemDto> Create(ShoppingItemCreateDto shoppingItemCreateDto, ClaimsPrincipal user)
        {
            if (shoppingItemCreateDto == null)
                throw new ArgumentNullException(nameof(shoppingItemCreateDto));
            if (string.IsNullOrWhiteSpace(shoppingItemCreateDto.Name))
                throw new ArgumentException("ItemName is required");

            var familyId = _claimsHandler.GetFamilyId(user);
            var userId = _claimsHandler.GetUserId(user);

            var shoppingItem = _mapper.Map<ShoppingItem>(shoppingItemCreateDto);
            
            shoppingItem.CreatorId = userId;
            shoppingItem.FamilyId = familyId; 
            await _context.ShoppingItems.AddAsync(shoppingItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(shoppingItem);
        }
        public async Task<List<ShoppingItemDto>> GetAll()
        {
            var shoppingItems = await _context.ShoppingItems
                .Include(si => si.Votes)
                .ToListAsync();
            return _mapper.Map<List<ShoppingItemDto>>(shoppingItems);
        }

        public async Task<List<ShoppingItemDto>> GetAllInFamily(ClaimsPrincipal user)
        {
           var familyId = _claimsHandler.GetFamilyId(user);

            var shoppingItems = await _context.ShoppingItems
                .Where(si => si.FamilyId == familyId)
                .Include(si => si.Votes)
                .ToListAsync();

            return _mapper.Map<List<ShoppingItemDto>>(shoppingItems);
        }

        public async Task<ShoppingItemDto> GetById(int id)
        {
            var item = _context.ShoppingItems
                   .Include(si => si.Votes)
                   .FirstOrDefault(si => si.Id == id);
            return _mapper.Map<ShoppingItemDto>(item);  
        }

        public async Task<ShoppingItemDto> Vote(int itemId, ClaimsPrincipal user)
        {
            var userId = _claimsHandler.GetUserId(user);
            var familyId = _claimsHandler.GetFamilyId(user);

            var item = await _context.ShoppingItems
                .FirstOrDefaultAsync(si => si.Id == itemId && si.FamilyId == familyId);
            if (item == null)
                throw new KeyNotFoundException("Shopping item not found in this family.");

            var vote = new ShoppingItemVote
            {
                ShoppingItemId = itemId,
                UserId = userId,
            };

            if(item.Votes.Any(v => v.UserId == userId))
                throw new InvalidOperationException("You have already voted for this item.");
            
            var familyMemberCount = await _context.FamilyUsers //Ha mindenki zavasz akkor az IsNeeded true lesz
                .Where(fu => fu.FamilyId == familyId)
                .CountAsync();

            item.Votes.Add(vote);
            if (item.Votes.Count == familyMemberCount)
                item.IsNeeded = true;
            
            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);
        }

        public async Task<ShoppingItemDto> Update(int id, ShoppingItemCreateDto updateDto, ClaimsPrincipal user )
        {
            if (string.IsNullOrWhiteSpace(updateDto.Name))
                throw new ArgumentException("ShoppingItem name is required");

            var familyId = _claimsHandler.GetFamilyId(user);
            var userRole = _claimsHandler.GetUserRole(user);
            var userId = _claimsHandler.GetUserId(user);

            var item = await _context.ShoppingItems
                .Include(si => si.Votes)
                .FirstOrDefaultAsync(si => si.Id == id && si.FamilyId == familyId);

            if (userRole != Roles.Admin && userRole != Roles.Owner && userId != item.CreatorId)
                throw new UnauthorizedAccessException("You do not have permission to update this shopping item.");

            item = _mapper.Map(updateDto, item);
            item.Votes.Clear();

            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);

        }

        public async Task<ShoppingItemDto> Delete(int id,ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var userId = _claimsHandler.GetUserId(user);
            var userRole = _claimsHandler.GetUserRole(user);
        

            var item = await _context.ShoppingItems
                .FirstOrDefaultAsync(si => si.Id == id && si.FamilyId == familyId);
            if (item.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to delete this task.");
            if (userRole != Roles.Admin && userRole != Roles.Owner && userId != item.CreatorId)
                throw new UnauthorizedAccessException("You do not have permission to update this shopping item.");

            _context.ShoppingItems.Remove(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);
        }

    }
}
