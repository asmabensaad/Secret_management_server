namespace Hangfireapp.Services;

public class ServiceManagement:IserviceManagement
{
    public void GenerateMerchandise()
    {
        Console.WriteLine($"Generate Merchandise: long running service at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }

    public void SendEmail()
    {
        Console.WriteLine($"Send Email: delayed execution service at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }

    public void SyncRecords()
    {
        Console.WriteLine($"Sync Records: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }

    public void RecurringJobsSendMail(string email)
    {
        Console.WriteLine("Mail sent");
    }

    public void UpdateDatabase()
    {
        Console.WriteLine($"Update Database: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
    }

}