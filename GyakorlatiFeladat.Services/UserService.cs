using AutoMapper;
using GyakorlatiFeladat.DataContext.Context;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using GyakorlatiFeladat.DataContext.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAll();
        public Task<UserDto> Create(UserDto user);
        public Task<UserDto> GetById(int id);
        public Task<UserDto> UpdateEmail(int id ,string email);
        public Task<UserDto> DeleteById(int id);
    }
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Create(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.FullName) || string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Username, Email, and Password are required");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists");

            var newUser = _mapper.Map<User>(user);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();


            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = findbyid(id);
            return _mapper.Map<UserDto>(user);
        }     
        
        public async Task<UserDto> UpdateEmail(int id, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty");
            var UserWithNewEmail = _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (UserWithNewEmail != null)
                throw new InvalidOperationException("A user with this email already exists");   

            User user = findbyid(id);          
            user.Email = email;
            _context.Update(user);
            _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> DeleteById(int id)
        {
            var user = findbyid(id);
            _context.Users.Remove(user);
            _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }


        //Belső függvény
        private User findbyid(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
