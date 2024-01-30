using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Helpers
{
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;
        public ReusableSql(IConfiguration config)
        {
            _dapper = new DataContextDapper(config); 
        }
        
        public bool UpsertUser(UserComplete user)
        {
            string query = @"EXEC TutorialAppSchema.spUser_Upsert 
                    @FirstName, @LastName, @Email, @Gender, @JobTitle, @Department, @Salary, @Active, @UserId;";
            var sqlParameters = new DynamicParameters();
            sqlParameters.Add("FirstName", user.FirstName, DbType.String);
            sqlParameters.Add("LastName", user.LastName, DbType.String);
            sqlParameters.Add("Email", user.Email, DbType.String);
            sqlParameters.Add("Gender", user.Gender, DbType.String);
            sqlParameters.Add("JobTitle", user.JobTitle, DbType.String);
            sqlParameters.Add("Department", user.Department, DbType.String);
            sqlParameters.Add("Salary", user.Salary, DbType.Decimal);
            sqlParameters.Add("Active", user.Active, DbType.Boolean);
            sqlParameters.Add("UserId", user.UserId, DbType.Int32);

            return _dapper.ExecuteSqlWithParameters(query, sqlParameters);
        }
    }
}