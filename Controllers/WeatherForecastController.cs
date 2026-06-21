using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TMS_API.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous] 
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Value 1", "Value 2", "API is working perfectly!" };
    }
}