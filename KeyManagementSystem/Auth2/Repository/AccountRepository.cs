using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auth2.Repository;

public class AccountRepository : IAccountRepository
{ private readonly IConfiguration _configuration;

    private readonly ApplicationDbContext _applicationDbContext;
    public AccountRepository(IConfiguration configuration , ApplicationDbContext applicationDbContext)
    {
        _configuration = configuration;
        _applicationDbContext = applicationDbContext;
    }
  


    public string getMyName()
    {
        throw new NotImplementedException();
    }

    public List<RegisterModel> GetUsers()
    {
        throw new NotImplementedException();
    }

    public RegisterModel Getuser(int id)
    {
        throw new NotImplementedException();
    }

    public List<RegisterModel> updateuser(int id, RegisterModel request)
    {
        throw new NotImplementedException();
    }
}