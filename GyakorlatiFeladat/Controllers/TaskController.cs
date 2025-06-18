using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemCreateDto taskItemCreateDto)
        {
            try
            {
                var result = await _taskItemService.CreateTask(taskItemCreateDto,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var result = await _taskItemService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpGet("familytasks")]
        public async Task<IActionResult> GetAllFamilyTasks()
        {
            try
            {
                var result = await _taskItemService.GetAllInFamily(User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var result = await _taskItemService.GetById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("mytasks/")]
        public async Task<IActionResult> GetTasksByLogedUserId()
        {
            try
            {
                var result = await _taskItemService.GetTaskByLogedUser(User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("userid/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            try
            {
                var result = await _taskItemService.GetTasksByUserId(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("check/{id}")]
        public async Task<IActionResult> Check(int id)
        {
            try
            {
                var result = await _taskItemService.Check(id,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemCreateDto taskItemCreateDto)
        {
            try
            {
                var result = await _taskItemService.UpdateTask(id, taskItemCreateDto,User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskItemService.DeleteTask(id,User);
                return Ok(result);
            }
            catch (Exception e)
            { 
                return BadRequest(e.Message);
            }
        }
    }
}
