using Auth2;

namespace DataAccess.Database.Repository;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
public interface IAccountRepository
{
    //public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    //public JwtSecurityToken CreateToken(List<Claim> authClaims);
    string getMyName();


    List<RegisterModel> GetUsers();
    RegisterModel Getuser(int id);
    List<RegisterModel> updateuser(int id, RegisterModel request);
}