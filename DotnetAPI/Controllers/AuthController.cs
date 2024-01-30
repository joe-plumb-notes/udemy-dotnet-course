using System.Security.Claims;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusableSql;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _reusableSql = new ReusableSql(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<UserForRegistrationDto, UserComplete>();
                }));
        }

        // Register user
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                // Check if user exists
                string query = $@"SELECT [Email]
                        FROM TutorialAppSchema.Auth 
                        WHERE [Email] = '{ userForRegistration.Email }'";
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(query);
                if (existingUsers.Count() == 0)
                {
                    UserForLoginDto userForSetPassword = new UserForLoginDto(){
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    };
                    if (_authHelper.SetPassword(userForSetPassword))
                    {
                        UserComplete userComplete = _mapper.Map<UserComplete>(userForRegistration);
                        userComplete.Active = true;
                           
                        if (_reusableSql.UpsertUser(userComplete))
                        {
                            return Ok();
                        }
                        throw new Exception ("Failed to add user");
                    }
                    throw new Exception ("Failed to register user");
                } 
                throw new Exception ("User with this Email already exists");
            }
            throw new Exception("Passwords do not match");
        }

        // Update password
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
        {
            if (_authHelper.SetPassword(userForSetPassword))
            {
                return Ok();
            }
            throw new Exception("Failed to update password!");
        }

        // Login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            // Get passwordhash and passwordsalt from db
            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get @Email";
            
            UserForLoginConfirmationDto userForLoginConfirmation;

            try
            {
                userForLoginConfirmation = _dapper.LoadDataSingleWithParams<UserForLoginConfirmationDto>(sqlForHashAndSalt, new {Email = userForLogin.Email});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound("User not found");
            }
           
            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }

            string sqlGetUserId = $@"SELECT UserId
                FROM TutorialAppSchema.Users
                WHERE [Email] = @Email";

            int userId = _dapper.LoadDataSingleWithParams<int>(sqlGetUserId, new {Email = userForLogin.Email});

            return Ok(new Dictionary<string, string>{
                    {"token", _authHelper.CreateToken(userId)}
                });
        }

        [HttpGet("refreshtoken")]
        public string RefreshToken()
        {
            // see if user id tied to the token is valid, and if so, use that id to create a new token and return to user
            string sqlGetUserId = @"SELECT UserId
                FROM TutorialAppSchema.Users
                WHERE [UserId] = @UserId";

            int userId = _dapper.LoadDataSingleWithParams<int>(sqlGetUserId, new { UserId = User.FindFirstValue("userId") });
            
            return _authHelper.CreateToken(userId);
        }

        
    }
}