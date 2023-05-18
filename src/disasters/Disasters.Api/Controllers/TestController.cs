using Disasters.Api.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    public record TestResponse(string output);
    
    [HttpGet(Name = "GetTest")]
    public TestResponse GetTest()
    {
        using var db = new DisastersDbContext();
        var disasters = db.TestView.ToList();

        return new TestResponse("");
    }
}