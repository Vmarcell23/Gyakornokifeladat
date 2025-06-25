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

namespace GyakorlatiFeladat.Services
{
    public interface ITaskItemService
    {
        Task<List<TaskItemDto>> GetAll();
        Task<List<TaskItemDto>> GetAllInFamily(ClaimsPrincipal user);
        Task<TaskItemDto> CreateTask(TaskItemCreateDto createDto,ClaimsPrincipal user);
        Task<TaskItemDto> Check(int id,ClaimsPrincipal user);
        Task<TaskItemDto> GetById(int id);
        Task<List<TaskItemDto>> GetTasksByUserId(int userId);
        Task<List<TaskItemDto>> GetTaskByLogedUser(ClaimsPrincipal user);
        Task<TaskItemDto> UpdateTask(int id, TaskItemCreateDto updateDto, ClaimsPrincipal user);
        Task<TaskItemDto> DeleteTask(int id,ClaimsPrincipal user);
    }
    public class TaskItemService : ITaskItemService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsHandler _claimsHandler;
        public TaskItemService(AppDbContext context, IMapper mapper,IClaimsHandler claimsHandler)
        {
            _context = context;
            _mapper = mapper;
            _claimsHandler = claimsHandler;
        }

        public async Task<TaskItemDto> CreateTask(TaskItemCreateDto creatDto, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(creatDto.TaskName) || string.IsNullOrWhiteSpace(creatDto.TaskDesc))
                throw new ArgumentException("TaskName and TaskDesc are required");
            
            var familyId = _claimsHandler.GetFamilyId(user);
            
            var taskItem = _mapper.Map<TaskItem>(creatDto);
            if (taskItem == null)
                throw new ArgumentNullException(nameof(creatDto));

            var allUsersOfFamily = await _context.FamilyUsers
                .Where(fu => fu.FamilyId == familyId && creatDto.UserIds.Contains(fu.UserId))
                .ToListAsync();

            var allUsersAssigned = await _context.Users
                .Where(u => creatDto.UserIds.Contains(u.Id))
                .ToListAsync();
            
            if(allUsersAssigned.Count() != allUsersOfFamily.Count())
                throw new UnauthorizedAccessException("You can only assign tasks to users who are members of your family.");

            taskItem.Users.AddRange(allUsersAssigned);
            taskItem.FamilyId = familyId;

            await _context.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskItemDto>(taskItem);

        }
        public async Task<List<TaskItemDto>> GetAll()
        {
            var taskItems = await _context.Tasks
                .Include(t => t.Users)
                .ToListAsync();
            return _mapper.Map<List<TaskItemDto>>(taskItems);
        }

        public async Task<List<TaskItemDto>> GetAllInFamily(ClaimsPrincipal user)
        {
          var familyId = _claimsHandler.GetFamilyId(user);

            var familyTasks = await _context.Tasks
                .Include(t => t.Users)
                .Where(t => t.FamilyId == familyId)
                .ToListAsync();

            return _mapper.Map<List<TaskItemDto>>(familyTasks);
        }

        public async Task<TaskItemDto> GetById(int id)
        {
            var taskItem = findbyid(id);
            return _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task<List<TaskItemDto>> GetTasksByUserId(int userId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.Users)
                .Where(t => t.Users.Any(u => u.Id == userId))
                .ToListAsync();
            return _mapper.Map<List<TaskItemDto>>(tasks);
        }

        public async Task<List<TaskItemDto>> GetTaskByLogedUser(ClaimsPrincipal user)
        {
            var userId = _claimsHandler.GetUserId(user);
            return await GetTasksByUserId(userId);
        }

        public async Task<TaskItemDto> Check(int id, ClaimsPrincipal user)
        {
            var userId = _claimsHandler.GetUserId(user);
            var familyId = _claimsHandler.GetFamilyId(user);

            var taskItem = await _context.Tasks
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == id && t.FamilyId == familyId);
            var usersTask = taskItem.Users.FirstOrDefault(u => u.Id == userId);
            if (usersTask == null && taskItem.Users.Count > 0)
                throw new UnauthorizedAccessException("You are not assigned to this task.");

            taskItem.IsDone = true;
            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskItemDto>(taskItem);
        }  

        public async Task<TaskItemDto> UpdateTask(int id, TaskItemCreateDto updateDto, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(updateDto.TaskName) || string.IsNullOrWhiteSpace(updateDto.TaskDesc))
                throw new ArgumentException("TaskName and TaskDesc are required");
            
            var familyId = _claimsHandler.GetFamilyId(user);

            var taskItem = findbyid(id);
            if (taskItem.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to update this task.");

            var userisfamlymember = await _context.FamilyUsers
                .Where(fu => fu.FamilyId == familyId && updateDto.UserIds.Contains(fu.UserId))
                .ToListAsync();
            var users = await _context.Users
                .Where(u => updateDto.UserIds.Contains(u.Id))
                .ToListAsync();
            if (users.Count() != userisfamlymember.Count())
                throw new UnauthorizedAccessException("You can only assign tasks to users who are members of your family.");

            taskItem = _mapper.Map(updateDto, taskItem);
            taskItem.Users.Clear();
            taskItem.Users.AddRange(users);

            _context.Update(taskItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task<TaskItemDto> DeleteTask(int id, ClaimsPrincipal user)
        {
            var familyId = _claimsHandler.GetFamilyId(user);
            var userRole = _claimsHandler.GetUserRole(user);
            var taskItem = findbyid(id);

            if (userRole != Roles.Owner && userRole != Roles.Admin)
                throw new UnauthorizedAccessException("You do not have permission to delete shopping items.");

            if (taskItem.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to delete this task.");

            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskItemDto>(taskItem);
        }

        //Belső függvény
        private TaskItem findbyid(int id )
        {
            var taskItem = _context.Tasks
                .Include(t => t.Users)
                .FirstOrDefault(t => t.Id == id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            return taskItem;
        }
    }
}
