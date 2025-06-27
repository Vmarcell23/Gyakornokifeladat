using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GyakorlatiFeladat.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDto recipeCreateDto)
        {
            try
            {
                var result = await _recipeService.Create(recipeCreateDto, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("recipes")]
        public async Task<IActionResult> GetAllRecipes()
        {
            try
            {
                var recipes = await _recipeService.GetAll();
                return Ok(recipes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("my-family/recipes")]
        public async Task<IActionResult> GetFamilyRecipes()
        {
            try
            {
                var recipes = await _recipeService.GetFamilyRecipes(User);
                return Ok(recipes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeCreateDto recipeCreateDto)
        {
            try
            {
                var result = await _recipeService.Update(id, recipeCreateDto, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                var result = await _recipeService.Delete(id, null, User);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
