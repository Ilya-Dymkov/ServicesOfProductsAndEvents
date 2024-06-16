using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ICategoryService _categoryService = new CategoryService(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll() => 
        Ok(await _categoryService.GetAll());

    private async Task<ActionResult<Category>> BaseActionGet(Func<Task<Category?>> func, string errorMessage)
    {
        var category = await func();

        if (category == null) throw new Exception(errorMessage);

        return Ok(category);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Category>> Get(string name) => 
        await BaseActionGet(() => _categoryService.Get(name), 
            $"There is no category with name '{name}'!");

    [HttpPost("{name}")]
    public async Task<ActionResult<Category>> Add(string name) =>
        await BaseActionGet(() => _categoryService.Add(name),
            $"The new category with name '{name}' could not be created!");

    [HttpPatch("{oldName}")]
    public async Task<ActionResult<Category>> UpdateName(string oldName, string newName) =>
        await BaseActionGet(() => _categoryService.UpdateName(oldName, newName),
            $"The category with name '{oldName}' to new name '{newName}' could not be updated!");

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await _categoryService.Delete(name);
        return Ok($"The category with name '{name}' has been deleted!");
    }
}