using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly DataContextDapper _dapper;
    private readonly ReusableSql _reusableSql;
    public UserCompleteController(ILogger<UserController> logger, IConfiguration config)
    {
        _logger = logger;
        _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);
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
            
            var sqlParameters = new DynamicParameters();
            sqlParameters.Add("UserId", userId, DbType.Int32);
            sqlParameters.Add("Active", isActive, DbType.Boolean);

            var user = _dapper.LoadDataWithParams<UserComplete>(query, sqlParameters);
            if (user == null)
            {
                return NotFound();
            }
            return Ok( user );
        }
    }

    [HttpPut]
    public IActionResult UpsertUser(UserComplete user)
    {
        _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + user.UserId + " was passed. Updating User Details from db..");

        if (_reusableSql.UpsertUser(user))
        {
            return Ok();
        }
        throw new Exception("Failed to update user");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string query = @$"EXEC TutorialAppSchema.spUser_Delete @UserId;";
        
        _logger.LogInformation("SQL to be executed on DB: "+ query);
        
        var sqlParameters = new DynamicParameters();
        sqlParameters.Add("UserId", userId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameters(query, sqlParameters))
        {
            return Ok();
        }
        else
        {
            return NotFound();
            throw new Exception($"Failed to delete user with Id: {userId}");
        }
    }
}
