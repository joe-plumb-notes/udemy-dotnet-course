using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    DataContextDapper _dapper;
    public UserCompleteController(ILogger<UserController> logger, IConfiguration config)
    {
        _logger = logger;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserComplete>> GetUsers(int? userId = null, bool? isActive = null)
    {
        if (userId == null && isActive == null)
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");

            string query = @"EXEC TutorialAppSchema.spUsers_Get";

            IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(query);
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");

            string query = @"EXEC TutorialAppSchema.spUsers_Get @UserId, @Active;";
            
            var parametersDict = new Dictionary<string, object>
            {
                {"UserId", userId},
                {"Active", isActive}
            };

            var sqlParameters = new DynamicParameters(parametersDict);

            var user = _dapper.LoadDataWithParams<UserComplete>(query, sqlParameters);
            if (user == null)
            {
                return NotFound();
            }
            return Ok( user );
        }
    }

    [HttpPut]
    public IActionResult EditUser(User user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + user.UserId + " was passed. Updating User Details from db..");

        string query = @"
            UPDATE TutorialAppSchema.Users 
                SET [FirstName] = '" + user.FirstName + @"'
                    , [LastName] = '" + user.LastName + @"'
                    , [Email] = '"+ user.Email +@"'
                    , [Gender] = '"+user.Gender+@"'
                    , [Active] = '"+user.Active+@"' 
                WHERE [UserId] = " + user.UserId + ";";
        _logger.LogInformation("SQL to be executed on DB: "+ query);
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }
        throw new Exception("Failed to update user");
    }

    [HttpPost]
    public IActionResult AddUser(UserDto user)
    {
         _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". Username " + user.FirstName + " "+ user.LastName + " was passed. Updating User Details from db..");
        
        string query = @"
            INSERT INTO TutorialAppSchema.Users (
                    [FirstName]
                    , [LastName]
                    , [Email]
                    , [Gender]
                    , [Active])
                VALUES
                    (
                        '" + user.FirstName + @"',
                        '" + user.LastName + @"',
                        '" + user.Email + @"',
                        '" + user.Gender + @"',
                        '" + user.Active + @"'
                    )  
                    ;";

        _logger.LogInformation("SQL to be executed on DB: "+ query);
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string query = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE [UserId] = " + userId + ";";
         _logger.LogInformation("SQL to be executed on DB: "+ query);
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }
        else
        {
            return NotFound();
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
    }

    [HttpPost("salary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". UserId " + userSalary.UserId + " was passed. Updating User Salary info in db..");
        
        string query = @"
            INSERT INTO TutorialAppSchema.UserSalary (
                    [UserId]
                    , [Salary])
                VALUES
                    (
                        " + userSalary.UserId + @",
                        " + userSalary.Salary + @"
                    )  
                    ;";

        _logger.LogInformation("SQL to be executed on DB: "+ query);
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
    }
}
