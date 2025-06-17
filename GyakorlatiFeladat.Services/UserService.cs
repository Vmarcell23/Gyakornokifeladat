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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace GyakorlatiFeladat.Services
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAll();
        public Task<UserDto> Create(UserRegisterDto user);
        public Task<string> Login(UserLoginDto user);
        public Task<UserDto> GetById(int id);
        public Task<UserDto> UpdateEmail(int id ,string email);
        public Task<UserDto> DeleteById(int id);
    }
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration; 

        }

        public async Task<UserDto> Create(UserRegisterDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.FullName) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Username, Email, and Password are required");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists");

            var newUser = _mapper.Map<User>(user);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();


            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<string> Login(UserLoginDto userDto)
        {
            var user = await _context.Users.Include(u => u.FamilyUsers)
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return await GenerateToken(user);

        }

        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var id = await GetCalimsIdentity(user);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], id.Claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private async Task<ClaimsIdentity> GetCalimsIdentity(User user)
        {
            var familyUser = user.FamilyUsers.FirstOrDefault();
            var familyId = user.FamilyUsers.FirstOrDefault()?.FamilyId ?? 0;
            var role = familyUser?.Role.ToString() ?? "None";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim("FamilyId", familyId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            return new ClaimsIdentity(claims, "Token");
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
