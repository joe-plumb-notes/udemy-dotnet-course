using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetUsers/{testValue}")]
    // public IEnumerable<User> GetUsers()
    public string[] GetUsers(string testValue)
    {
        _logger.LogInformation("C# User endpoint processed a request at " + DateTime.Now + ".");

        return new string[] {"joe", "amy", testValue };
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
    }
}
