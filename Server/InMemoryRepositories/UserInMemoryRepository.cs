using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public UserInMemoryRepository()
    {
        AddDummyData();
    }

    private void AddDummyData()
    {
        users.Add(new User {Id = 1,Password = "12345",Username = "User1"});
        users.Add(new User {Id = 2,Password = "12345",Username = "User2"});
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(p => (int)p.Id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }
    
    public Task UpdateAsync(User user)
    {
        User? existinguser = users.SingleOrDefault(p => p.Id == user.Id);
        if (existinguser is null)
        {
            throw new InvalidOperationException(
                $"user with ID '{user.Id}' not found");
        }

        users.Remove(existinguser);
        users.Add(user);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"user with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }
    
    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(p => p.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"user with ID '{id}' not found");
        }
        return Task.FromResult(user);
    }
    
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}