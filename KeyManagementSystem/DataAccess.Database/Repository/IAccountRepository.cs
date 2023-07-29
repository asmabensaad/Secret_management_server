using Auth2;

namespace DataAccess.Database.Repository;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
public interface IAccountRepository
{
    
    string GetMyName();


    List<RegisterModel> GetUsers();
}