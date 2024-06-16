using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.Controllers.ControllersSource;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly IUserService _userService = new UserService(dbContext);
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll() => 
        Ok(await _userService.GetAll());

    [HttpGet("{login}")]
    public async Task<ActionResult<User>> Get(string login) => 
        await this.BaseActionGet(() => _userService.Get(login), 
            $"There is no user with login '{login}'!");
    
    [HttpGet("{login}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string login) =>
        await this.BaseActionGet(() => _userService.GetOrders(login)!,
            $"There is no user with login '{login}'!");

    [HttpGet("{login}/transactions")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(string login) =>
        await this.BaseActionGet(() => _userService.GetTransactions(login)!,
            $"There is no user with login '{login}'!");

    [HttpPost("{login}")]
    public async Task<ActionResult<User>> Add(string login, string name,
        Genders gender, DateTime birthday, bool isAdmin) =>
        await this.BaseActionGet(() => _userService.Add(login, name, gender, birthday, isAdmin),
            $"The new user with login '{login}' could not be created!");

    [HttpPatch("{oldLogin}")]
    public async Task<ActionResult<User>> UpdateLogin(string oldLogin, string newLogin) =>
        await this.BaseActionGet(() => _userService.UpdateLogin(oldLogin, newLogin),
            $"The user with login '{oldLogin}' to new login '{newLogin}' could not be updated!");
    
    [HttpPatch("{login}/data")]
    public async Task<ActionResult<User>> UpdateData(string login, string name,
        Genders gender, DateTime birthday, bool isAdmin) =>
        await this.BaseActionGet(() => _userService.UpdateData(login, name, gender, birthday, isAdmin),
            $"The data of user with login '{login}' could not be updated!");

    [HttpPut("{login}")]
    public async Task<ActionResult<User>> Recovery(string login) =>
        await this.BaseActionGet(() => _userService.Recovery(login),
            $"The user with login '{login}' could not be recovered!");
    
    [HttpDelete("{login}")]
    public async Task<IActionResult> SoftDelete(string login)
    {
        await _userService.SoftDelete(login);
        return Ok($"The user with login '{login}' has been deleted!");
    }
    
    [HttpDelete("{login}/hard")]
    public async Task<IActionResult> HardDelete(string login)
    {
        await _userService.HardDelete(login);
        return Ok($"The user with login '{login}' has been full deleted!");
    }
}