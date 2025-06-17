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
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.Services
{
    public interface IShoppingItemService
    {
        Task <List<ShoppingItemDto>> GetAll();
        Task <ShoppingItemDto> GetById(int id);
        Task<ShoppingItemDto> Create(ShoppingItemCreateDto shoppingItemCreateDto);
        Task<ShoppingItemDto> Vote(ShoppingItemCreateVoteDto voteDto);
        Task<ShoppingItemDto> Update(int id, ShoppingItemCreateDto updateDto);
        Task<ShoppingItemDto> Delete(int id);
     

    }
    public class ShoppingItemService : IShoppingItemService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ShoppingItemService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ShoppingItemDto> Create(ShoppingItemCreateDto shoppingItemCreateDto)
        {
            if (shoppingItemCreateDto == null)
                throw new ArgumentNullException(nameof(shoppingItemCreateDto));
            if (string.IsNullOrWhiteSpace(shoppingItemCreateDto.Name))
                throw new ArgumentException("ItemName is required");

            var shoppingItem = _mapper.Map<ShoppingItem>(shoppingItemCreateDto);
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

        public async Task<ShoppingItemDto> GetById(int id)
        {
            var item = findbyid(id);
            return _mapper.Map<ShoppingItemDto>(item);  
        }

        public async Task<ShoppingItemDto> Vote(ShoppingItemCreateVoteDto voteDto)
        {
            var item = findbyid(voteDto.ShoppingItemId);
            var vote = _mapper.Map<ShoppingItemVote>(voteDto);

            var uservoted = item.Votes.FirstOrDefault(v => v.UserId == vote.UserId);
            if (uservoted != null)
                throw new InvalidOperationException("This user has already voted for this item.");

            item.Votes.Add(vote);

            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);
        }

        public async Task<ShoppingItemDto> Update(int id, ShoppingItemCreateDto updateDto)
        {
            var item = findbyid(id);
            item = _mapper.Map(updateDto, item);

            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);

        }

        public async Task<ShoppingItemDto> Delete(int id)
        {
            var item = findbyid(id);
            _context.ShoppingItems.Remove(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShoppingItemDto>(item);
        }

        private ShoppingItem findbyid(int id)
        { 
            var item = _context.ShoppingItems
                .Include(si => si.Votes)
                .FirstOrDefault(si => si.Id == id);
            if (item == null)
                throw new KeyNotFoundException($"Shopping item with ID {id} not found.");

            return item;
        }
    }
}
