using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth2.Repository;

public interface IAccountRepository
{
    
   //public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
   //public JwtSecurityToken CreateToken(List<Claim> authClaims);
    string getMyName();


    List<RegisterModel> GetUsers();
    RegisterModel Getuser(int id);
    List<RegisterModel> updateuser(int id, RegisterModel request);
}