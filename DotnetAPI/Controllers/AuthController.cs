using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
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
                    byte[] passwordSalt = new byte[128/8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);
                    
                    // SQL query with parameters for the hash and salt
                    string sqlAddAuth = $@"INSERT INTO TutorialAppSchema.Auth
                    ([Email],
                    [PasswordHash],
                    [PasswordSalt]) VALUES
                    ('{ userForRegistration.Email }', @PasswordHash, @PasswordSalt);";

                    // Create list of SQL parameters to pass in with query
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary)
                    {
                        Value = passwordHash
                    };
                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary)
                    {
                        Value = passwordSalt
                    };
                    // sqlParameters.Add(passwordHashParameter);
                    // sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.AddRange(new[] { passwordHashParameter, passwordSaltParameter });

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        string sqlAddUserFromRegistration = @"
                            INSERT INTO TutorialAppSchema.Users (
                                    [FirstName]
                                    , [LastName]
                                    , [Email]
                                    , [Gender]
                                    , [Active])
                                VALUES
                                    (
                                        '" + userForRegistration.FirstName + @"',
                                        '" + userForRegistration.LastName + @"',
                                        '" + userForRegistration.Email + @"',
                                        '" + userForRegistration.Gender + @"',
                                        1
                                    )  
                                    ;";
                        if (_dapper.ExecuteSql(sqlAddUserFromRegistration))
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

        // Login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            // Get passwordhash and passwordsalt from db
            string sqlForHashAndSalt = @"SELECT [PasswordHash], [PasswordSalt]
                FROM TutorialAppSchema.Auth
                WHERE [Email] = @Email";
            
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
            string sqlGetUserId = $@"SELECT UserId
                FROM TutorialAppSchema.Users
                WHERE [UserId] = @UserId";

            int userId = _dapper.LoadDataSingleWithParams<int>(sqlGetUserId, new { UserId = User.FindFirstValue("userId") });
            
            return _authHelper.CreateToken(userId);
        }

        
    }
}