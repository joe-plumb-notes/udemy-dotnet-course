using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        DataContextEF _entityFramework;
        
        public UserRepository(ILogger<UserRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");
            return _entityFramework.Users.ToList();
        }
        
        public User GetUserById(int userId)
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");
            User? user = _entityFramework.Users.Find(userId);
            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to get user");
        }

        public IEnumerable<UserSalary> GetAllUserSalaries()
        {
            _logger.LogInformation("Users Salary endpoint processed a request at " + DateTime.Now + ". Getting all users salaries from the db..");
            IEnumerable<UserSalary> users = _entityFramework.UserSalary.ToList();
            decimal avgSalary = Math.Round(_entityFramework.UserSalary.Average(user => user.Salary), 2);
            foreach (var user in users)
            {
                user.AvgSalary = avgSalary;
            }
            return users;
        }
        
        public UserSalary GetUserSalaryById(int userId)
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");
            UserSalary? userSalary = _entityFramework.UserSalary.Find(userId);
            if (userSalary != null)
            {
                decimal avgSalary = Math.Round(_entityFramework.UserSalary.Average(user => user.Salary), 2);
                userSalary.AvgSalary = avgSalary;
                return userSalary;
            }       
            throw new Exception($"Failed to get salary info for user { userId }");
        }

        public IEnumerable<UserJobInfo> GetAllUserJobInfo()
        {
            _logger.LogInformation("Users Salary endpoint processed a request at " + DateTime.Now + ". Getting all users salaries from the db..");
            IEnumerable<UserJobInfo> users = _entityFramework.UserJobInfo.ToList();
            return users;
        }
        
        public UserJobInfo GetUserJobInfoById(int userId)
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Job Info from db..");
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo.Find(userId);
            if (userJobInfo != null)
            {
                return userJobInfo;
            }       
            throw new Exception($"Failed to get salary info for user { userId }");
        }
        
    }
}