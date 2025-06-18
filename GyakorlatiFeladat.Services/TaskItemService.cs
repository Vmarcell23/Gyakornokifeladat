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
        // Define methods for task management here, e.g.:
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
        public TaskItemService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> CreateTask(TaskItemCreateDto creatDto, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(creatDto.TaskName) || string.IsNullOrWhiteSpace(creatDto.TaskDesc))
                throw new ArgumentException("TaskName and TaskDesc are required");

            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            var familyId = int.Parse(familyIdClaim.Value);
            if (familyIdClaim == null)
                throw new UnauthorizedAccessException("You are not a member of any family.");

            var taskItem = _mapper.Map<TaskItem>(creatDto);
            if (taskItem == null)
                throw new ArgumentNullException(nameof(creatDto));


            var userisfamlymember = _context.FamilyUsers
                .Where(u => creatDto.UserIds.Contains(u.Id));

            var users = await _context.Users
                .Where(u => creatDto.UserIds.Contains(u.Id))
                .ToListAsync();
            
            if(users.Count() != userisfamlymember.Count())
                throw new UnauthorizedAccessException("You can only assign tasks to users who are members of your family.");

            taskItem.Users.AddRange(users);
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
            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            if (familyIdClaim == null)
                throw new UnauthorizedAccessException("You are not a member of any family.");
            var familyId = int.Parse(familyIdClaim.Value);
            
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
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException();
            int userId = int.Parse(userIdClaim.Value);
            return await GetTasksByUserId(userId);
        }

        public async Task<TaskItemDto> Check(int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("You are not logged in.");
            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            if (familyIdClaim == null)
                throw new UnauthorizedAccessException("You are not a member of any family.");
            
            var userId = int.Parse(userIdClaim.Value);
            var familyId = int.Parse(familyIdClaim.Value);
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

            var FamilyClaimId = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            var familyId = int.Parse(FamilyClaimId.Value);
            var taskItem = findbyid(id);
            if (taskItem.FamilyId != familyId)
                throw new UnauthorizedAccessException("You do not have permission to update this task.");

            var userisfamlymember = _context.FamilyUsers
                .Where(u => updateDto.UserIds.Contains(u.Id));
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
            var familyIdClaim = user.Claims.FirstOrDefault(c => c.Type == "FamilyId");
            var familyId = int.Parse(familyIdClaim.Value);

            var taskItem = findbyid(id);
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
