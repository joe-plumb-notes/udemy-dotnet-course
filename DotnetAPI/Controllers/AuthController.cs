using System.Data;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        // Register user
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

                    byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);
                    
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
        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
           // Get passwordhash and passwordsalt from db
           string sqlForHashAndSalt = $@"SELECT [PasswordHash], [PasswordSalt]
                FROM TutorialAppSchema.Auth
                WHERE [Email] = '{ userForLogin.Email }'";
            
            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);
           
            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            return Ok();
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            // Generate passwordsalt with AppSetting and database stored salt
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value 
                            + Convert.ToBase64String(passwordSalt);

            // hash the password
            return KeyDerivation.Pbkdf2(
                        password: password, 
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256/8);
        }
    }
}