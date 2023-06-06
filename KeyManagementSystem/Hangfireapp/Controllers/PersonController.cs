using Hangfire;
using Hangfireapp.Models;
using Hangfireapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hangfireapp.Controllers;
[ApiController]
[Route("[controller]")]
public class PersonController : Controller
{
    private static List<Person> person = new List<Person>();

    // private readonly ILogger<PersonController> _logger;
    private readonly IserviceManagement _serviceManagement;


    public PersonController(IserviceManagement serviceManagement)
    {
        _serviceManagement = serviceManagement;
    }

    [HttpGet]
   public void RecurringJobsSendMail()
    {
        RecurringJob.AddOrUpdate(
            "myrecurringjob",
            () => _serviceManagement.RecurringJobsSendMail("asma@gmail.com"),
            Cron.Minutely);
    }

    [HttpPost]
    public IActionResult CreatePerson(Person data)
    {
        if (ModelState.IsValid)
        {
            person.Add(data);
            //Fire and Forget JOb
            var jobId = BackgroundJob.Enqueue<IserviceManagement>(x => x.SendEmail());
            Console.WriteLine($"Job id :{jobId}");
            return CreatedAtAction("GetPerson", new { data.Id }, data);
        }

        return new JsonResult("something went wrong") { StatusCode = 500 };
    }

    [HttpGet("{id}")]
    public IActionResult GetPerson(int id)
    {
        var item = person.FirstOrDefault(x => x.Id == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }
}