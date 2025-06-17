using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
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
        Task<ShoppingItemDto> Create(ShoppingItemCreateDto shoppingItemCreateDto);

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

        public async Task<List<ShoppingItemDto>> GetAll()
        {
            var shoppingItems = await _context.ShoppingItems
                .Include(si => si.Votes)
                .ToListAsync();
            return _mapper.Map<List<ShoppingItemDto>>(shoppingItems);
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
    }
}
