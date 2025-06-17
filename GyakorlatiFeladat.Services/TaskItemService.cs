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
    public interface ITaskItemService
    {
        // Define methods for task management here, e.g.:
         Task<List<TaskItemDto>> GetAll();
        Task<TaskItemDto> CreateTask(TaskItemCreateDto createDto);
        Task<TaskItemDto> Check(int id);
        Task<TaskItemDto> GetById(int id);
        Task<List<TaskItemDto>> GetTasksByUserId(int userId);
        Task<TaskItemDto> UpdateTask(int id, TaskItemCreateDto updateDto);
        Task<TaskItemDto> DeleteTask(int id);
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

        public async Task<TaskItemDto> CreateTask(TaskItemCreateDto creatDto)
        {
            var taskItem = _mapper.Map<TaskItem>(creatDto);
            if (taskItem == null)
                throw new ArgumentNullException(nameof(creatDto));
            if (string.IsNullOrWhiteSpace(taskItem.TaskName) || string.IsNullOrWhiteSpace(taskItem.TaskDesc))
                throw new ArgumentException("TaskName and TaskDesc are required");

            var users = await _context.Users
                .Where(u => creatDto.UserIds.Contains(u.Id))
                .ToListAsync();
            taskItem.Users.AddRange(users);

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

        public async Task<TaskItemDto> Check(int id)
        {
            var taskItem = findbyid(id);
            taskItem.IsDone = true;
            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskItemDto>(taskItem);
        }  

        public async Task<TaskItemDto> UpdateTask(int id, TaskItemCreateDto updateDto)
        {
            var taskItem = findbyid(id);
            taskItem = _mapper.Map(updateDto, taskItem);
            
            var users = await _context.Users
              .Where(u => updateDto.UserIds.Contains(u.Id))
              .ToListAsync();
           
            taskItem.Users.Clear();
            taskItem.Users.AddRange(users);
            _context.Update(taskItem);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskItemDto>(taskItem);

        }

        public async Task<TaskItemDto> DeleteTask(int id)
        {
            var taskItem = findbyid(id);
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
