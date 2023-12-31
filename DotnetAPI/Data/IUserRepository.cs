using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public IEnumerable<User> GetAllUsers();
        public User GetUserById(int userId);
        public IEnumerable<UserSalary> GetAllUserSalaries();
        public UserSalary GetUserSalaryById(int userId);
        public IEnumerable<UserJobInfo> GetAllUserJobInfo();
        public UserJobInfo GetUserJobInfoById(int userId);
        
    }
}