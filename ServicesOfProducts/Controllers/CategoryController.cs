using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.Controllers.ControllersSource;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Proxies;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ICategoryService _categoryService = new CategoryServiceProxy(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll() => 
        Ok(await _categoryService.GetAll());

    [HttpGet("{name}/subsidiary")]
    public async Task<ActionResult<IEnumerable<Category>>> GetSubsidiary(string name) =>
        Ok(await _categoryService.GetSubsidiary(name));

    [HttpGet("{name}")]
    public async Task<ActionResult<Category>> Get(string name) => 
        await this.BaseActionGet(() => _categoryService.Get(name), 
            $"There is no category with name '{name}'!");
    
    [HttpGet("{name}/products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string name) => 
        await this.BaseActionGet(() => _categoryService.GetProducts(name)!, 
            $"There is no category with name '{name}'!");

    [HttpPost("{name}")]
    public async Task<ActionResult<Category>> Add(string name) =>
        await this.BaseActionGet(() => _categoryService.Add(name, null),
            $"The new category with name '{name}' could not be created!");
    
    [HttpPost("{parentName}/{name}")]
    public async Task<ActionResult<Category>> AddWithParent(string parentName, string name) =>
        await this.BaseActionGet(() => _categoryService.Add(name, parentName),
            $"The new category with name '{name}' could not be created!");

    [HttpPatch("{oldName}")]
    public async Task<ActionResult<Category>> UpdateName(string oldName, string newName) =>
        await this.BaseActionGet(() => _categoryService.UpdateName(oldName, newName),
            $"The category with name '{oldName}' to new name '{newName}' could not be updated!");

    [HttpPatch("{parentName}/{name}")]
    public async Task<ActionResult<Category>> UpdateParent(string parentName, string name) =>
        await this.BaseActionGet(() => _categoryService.UpdateParent(name, parentName),
            $"The category with name '{name}' to new parent with name {parentName} could not be updated!");

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await _categoryService.Delete(name);
        return Ok($"The category with name '{name}' has been deleted!");
    }
}