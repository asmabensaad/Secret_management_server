using Hangfire;
using Hangfireapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace Hangfireapp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController : Controller
{
    private readonly IDatabase _database;
    private readonly IserviceManagement _serviceManagement;
    public CacheController(IDatabase database, IserviceManagement servicemanagement)
    {
        _database = database;
        _serviceManagement = servicemanagement;
    }
    
    //GET : api/Cache/5
   
    [HttpGet("{key}")]  
    public async Task GetAsync(string key) {  
        await _database.StringGetAsync(key);  
    }  

  
    // Post:api/cache
    
    [HttpPost]  
    public async Task PostAsync([FromBody] KeyValuePair < string, string > keyValue) {  
        await _database.StringSetAsync(keyValue.Key, keyValue.Value);  
    }   
    
    [HttpPost(template: "expiry")]  
    public async Task SaveExpiry([FromBody] KeyValuePair < string, string > keyValue) {  
        await _database.StringSetAsync(keyValue.Key, keyValue.Value, expiry: TimeSpan.FromSeconds(10));  
    }   
    
    
    [HttpGet]
   public void RecurringJobsSendMail()
    {
       RecurringJob.AddOrUpdate(
            "myrecurringjob",
            () => _serviceManagement.RecurringJobsSendMail("asma@gmail.com"),
            Cron.Minutely);
    }
    
    }
   
