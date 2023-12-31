using System.Linq.Expressions;
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

    IUserRepository _userRepository;
    public UserEFController(IUserRepository userRepository, ILogger<UserEFController> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
        
        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers(int? userId = null)
    {
        if (userId.HasValue)
        {
            try
            {
                _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");
                return Ok(_userRepository.GetUserById(userId.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user details.");
                return NotFound();
            }
        }
        else
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");
            IEnumerable<User> users = _userRepository.GetAllUsers();
            return Ok(users);
        }
    }

    [HttpPost]
    public IActionResult AddUser(UserDto user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". Adding User Details to db..");

        User userDb = _mapper.Map<User>(user);

        _userRepository.AddEntity(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        
        throw new Exception("Failed to add user");
    }

    [HttpPut]
    public IActionResult EditUser(User user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + user.UserId + " was passed. Updating User Details from db..");

        User? userDb = _userRepository.GetUserById(user.UserId);
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
        else
        {
            return NotFound();
        }
        throw new Exception("Failed to update user");
    }    

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetUserById(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges())
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
        if (userId.HasValue)
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Salary Details from db..");
            UserSalary? userSalary = _userRepository.GetUserSalaryById(userId.Value);
            if (userSalary == null)
            {
                return NotFound();
            }
            return Ok(userSalary);
        }
        else
        {
            _logger.LogInformation("Users Salary endpoint processed a request at " + DateTime.Now + ". Getting all users salaries from the db..");
            return Ok(_userRepository.GetAllUserSalaries());
        }
    }

    [HttpPost("salary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". UserId " + userSalary.UserId + " was passed. Updating User Salary info in db..");
        
        try
        {
            UserSalary userSalaryToAdd = _mapper.Map<UserSalary>(userSalary);
            _userRepository.AddEntity(userSalaryToAdd);
            if (_userRepository.SaveChanges())
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

        UserSalary? userSalaryDb = _userRepository.GetUserSalaryById(userSalaryUpdate.UserId);
        if (userSalaryDb != null)
        {
            _mapper.Map(userSalaryUpdate, userSalaryDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update User Salary on Save");
        }
        return NotFound();
        throw new Exception("Failed to find user");
    }

    [HttpDelete("salary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryDb = _userRepository.GetUserSalaryById(userId);

        if (userSalaryDb != null)
        {
            _userRepository.RemoveEntity(userSalaryDb);
            if (_userRepository.SaveChanges())
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

    [HttpGet("jobinfo")]
    public ActionResult<IEnumerable<User>> GetUserJobInfo(int? userId = null)
    {
        if (userId.HasValue)
        {
            try
            {
                _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");
                return Ok(_userRepository.GetUserJobInfoById(userId.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user details.");
                return NotFound();
            }
        }
        else
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");
            IEnumerable<UserJobInfo> users = _userRepository.GetAllUserJobInfo();
            return Ok(users);
        }
    }

    [HttpPost("jobinfo")]
    public IActionResult AddUserJobInfo(UserJobInfo user)
    {
        _logger.LogInformation("User/jobinfo endpoint processed a request at " + DateTime.Now + ". Adding User Job Info to db..");

        UserJobInfo userDb = _mapper.Map<UserJobInfo>(user);

        _userRepository.AddEntity(userDb);
        try
        {
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                return Conflict($"Job info for user UserId { user.UserId } already exists.");
            }
            throw;
        }
        throw new Exception("Failed to add user job info to db");
    }

    [HttpPut("jobinfo")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfoUpdate)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userJobInfoUpdate.UserId + " was passed. Updating User job info in db..");

        UserJobInfo? userJobInfoDb = _userRepository.GetUserJobInfoById(userJobInfoUpdate.UserId);
        if (userJobInfoDb != null)
        {
            _mapper.Map(userJobInfoUpdate, userJobInfoDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update User Salary on Save");
        }
        return NotFound();
        throw new Exception("Failed to find user");
    }


    [HttpDelete("jobinfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
       UserJobInfo? userJobInfoDb = _userRepository.GetUserJobInfoById(userId);

        if (userJobInfoDb != null)
        {
            _userRepository.RemoveEntity(userJobInfoDb);
            if (_userRepository.SaveChanges())
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
