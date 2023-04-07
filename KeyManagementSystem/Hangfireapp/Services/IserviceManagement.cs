namespace Hangfireapp.Services;

public interface IserviceManagement
{
    
    void SendEmail();
    void UpdateDatabase();
    void GenerateMerchandise();
    void SyncRecords();
    void RecurringJobsSendMail(string email);

}
