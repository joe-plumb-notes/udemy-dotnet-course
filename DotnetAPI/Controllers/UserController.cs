using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    DataContextDapper _dapper;
    public UserController(ILogger<UserController> logger, IConfiguration config)
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
    public ActionResult<IEnumerable<User>> GetUsers(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");

            string query = @"
                SELECT  [UserId]
                    , [FirstName]
                    , [LastName]
                    , [Email]
                    , [Gender]
                    , [Active]
                FROM TutorialAppSchema.Users";

            IEnumerable<User> users = _dapper.LoadData<User>(query);
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");

            string query = @"
            SELECT  [UserId]
                , [FirstName]
                , [LastName]
                , [Email]
                , [Gender]
                , [Active]
                FROM TutorialAppSchema.Users 
                WHERE [UserId] = " + userId.ToString() + ";";

            var user = _dapper.LoadDataSingle<User>(query);
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

    [HttpGet("salary")]
    public ActionResult<IEnumerable<UserSalary>> GetUsersSalary(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("UsersSalary endpoint processed a request at " + DateTime.Now + ". Getting all Users Salary from the db..");

            string querySalaries = @"
                SELECT  [UserId]
                        , [Salary]
                        , (SELECT ROUND(AVG(Salary), 2) FROM TutorialAppSchema.UserSalary) AS AvgSalary
                FROM TutorialAppSchema.UserSalary";
            IEnumerable<UserSalary> users = _dapper.LoadData<UserSalary>(querySalaries);
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");

            string query = @"
            SELECT  ISNULL([UserId], NULL) AS UserId
                    , ISNULL([Salary], NULL) AS Salary
                    , (SELECT ROUND(AVG(Salary), 2) FROM TutorialAppSchema.UserSalary) AS AvgSalary
                FROM TutorialAppSchema.UserSalary 
                WHERE [UserId] = " + userId.ToString() + ";";
            try
            {
                var user = _dapper.LoadDataSingle<UserSalary>(query);
                if (user.UserId == null)
                {
                    return NotFound();
                }
                return Ok(new List<UserSalary> { user });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
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
