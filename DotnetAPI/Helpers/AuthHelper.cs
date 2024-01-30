using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public AuthHelper(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
        } 
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
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

        public string CreateToken(int userId)
        {
            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
    
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                )
            ); 

            SigningCredentials credentials = new SigningCredentials(tokenKey, 
                SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = credentials,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };
            
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(descriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public bool SetPassword(UserForLoginDto userForSetPassword)
        {
            byte[] passwordSalt = new byte[128/8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);
            
            // SQL query with parameters for the hash and salt
            string sqlAddAuth = $@"EXEC TutorialAppSchema.spRegistration_Upsert
            @Email,
            @PasswordHash,
            @PasswordSalt;";                   

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@Email", userForSetPassword.Email, DbType.String);
            sqlParameters.Add("@PasswordHash", passwordHash, DbType.Binary);
            sqlParameters.Add("@PasswordSalt", passwordSalt, DbType.Binary);

            return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
        }

    }

}