using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    private readonly ILogger<UserEFController> _logger;

    IMapper _mapper;

    DataContextEF _entityFramework;
    public UserEFController(ILogger<UserEFController> logger, IConfiguration config)
    {
        _logger = logger;
        _entityFramework = new DataContextEF(config);
        
        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserDto, User>();
        }));
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");

            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");

            User? user = _entityFramework.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new List<User> { user });
        }
    }

    [HttpPut]
    public IActionResult EditUser(User user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + user.UserId + " was passed. Updating User Details from db..");

        User? userDb = _entityFramework.Users.Find(user.UserId);
        if (userDb == null)
        {
            return NotFound();
        }
        
        else
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to update user");
    }

    [HttpPost]
    public IActionResult AddUser(UserDto user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". Adding User Details to db..");

        User userDb = _mapper.Map<User>(user);

        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        
        throw new Exception("Failed to add user");
    }
    

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users.Find(userId);

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
        else
        {
            return NotFound();
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
    }

    [HttpGet("salary")]
    public ActionResult<IEnumerable<UserSalary>> GetUsersSalary(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("Users Salary endpoint processed a request at " + DateTime.Now + ". Getting all users salaries from the db..");

            IEnumerable<UserSalary> users = _entityFramework.UserSalary.ToList<UserSalary>();
            // Calculate the average salary
            decimal avgSalary = Math.Round(_entityFramework.UserSalary.Average(user => user.Salary), 2);
            
            // Set the AvgSalary property for each user
            foreach (var user in users)
            {
                user.AvgSalary = avgSalary;
            }
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Salary Details from db..");

            UserSalary? userSalary = _entityFramework.UserSalary.Find(userId);
            if (userSalary == null)
            {
                return NotFound();
            }

            // Calculate the average salary
            decimal avgSalary = Math.Round(_entityFramework.UserSalary.Average(user => user.Salary), 2);

            userSalary.AvgSalary = avgSalary;

            return Ok(new List<UserSalary> { userSalary });
        }
    }

    [HttpPost("salary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". UserId " + userSalary.UserId + " was passed. Updating User Salary info in db..");
        
        try
        {
            UserSalary userSalaryToAdd = _mapper.Map<UserSalary>(userSalary);
            _entityFramework.Add(userSalaryToAdd);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                return Conflict("A user salary with the same UserId already exists.");
            }
            throw;
        }
        
        throw new Exception("Failed to add user salary info");
    }

    [HttpPut("salary")]
    public IActionResult EditUserSalary(UserSalary userSalaryUpdate)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userSalaryUpdate.UserId + " was passed. Updating User Salary in db..");

        UserSalary? userSalaryDb = _entityFramework.UserSalary.Find(userSalaryUpdate.UserId);
        if (userSalaryDb == null)
        {
            return NotFound();
        }
        
        else
        {
            userSalaryDb.UserId = userSalaryUpdate.UserId;
            userSalaryDb.Salary = userSalaryUpdate.Salary;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to update user");
    }

    [HttpDelete("salary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
       UserSalary? userSalaryDb = _entityFramework.UserSalary.Find(userId);

        if (userSalaryDb != null)
        {
            _entityFramework.UserSalary.Remove(userSalaryDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
        else
        {
            return NotFound();
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
    }

}
