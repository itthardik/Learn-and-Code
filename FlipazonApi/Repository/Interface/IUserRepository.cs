using FlipazonApi.Models;

namespace FlipazonApi.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<int> AddUser(string email, string password);
        public Task<User?> GetUser(string email);
    }
}
