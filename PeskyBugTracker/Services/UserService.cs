using PeskyBugTracker.Models;

namespace PeskyBugTracker.Services
{

    public interface IUserService
    {
        Task<Agent> Authenticate(string username, string password);

        Task<IEnumerable<Agent>> GetAll();

    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Agent> _users = new List<Agent>
    {
        new Agent { Id = Guid.NewGuid(), Forename = "Test", SurnameName = "User", UserName = "test", Password = "test" }
    };

        public async Task<Agent> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" to mimic fetching user from a db
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.UserName == username && x.Password == password));

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return user;
        }

        public async Task<IEnumerable<Agent>> GetAll()
        {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            return await Task.Run(() => _users);
        }
    }
}
