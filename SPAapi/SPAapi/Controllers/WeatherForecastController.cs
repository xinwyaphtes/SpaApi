using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SPAService;

namespace SPAapi.Controllers
{
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private IService1 _service;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IService1 service)
    {
      _logger = logger;
      _service = service;
    }

    //[HttpGet]
    //public IEnumerable<WeatherForecast> Get()
    //{
    //  var rng = new Random();
    //  return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //  {
    //    Date = DateTime.Now.AddDays(index),
    //    TemperatureC = rng.Next(-20, 55),
    //    Summary = Summaries[rng.Next(Summaries.Length)]
    //  })
    //  .ToArray();
    //}

    [HttpGet("GetSingle")]
    public IActionResult GetSingle([FromBody]JsonElement m1)
    {
      var input = JsonSerializer.Deserialize<RequestBody>(JsonSerializer.Serialize(m1));
      //return Ok(new { id = 1 });
      return Ok(_service.ServiceTest(input.Id));
    }
  }
}
