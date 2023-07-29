using Auth2;

namespace DataAccess.Database.Repository;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public AccountRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    public string GetMyName()
    {
        throw new NotImplementedException();
    }

    public List<RegisterModel> GetUsers()
    {
        throw new NotImplementedException();
    }

    public LoginModel GetUser(string email)
    {
        throw new NotImplementedException();
    }

    public List<RegisterModel> UpdateUser(int id, RegisterModel request)
    {
        throw new NotImplementedException();
    }
    
    
}