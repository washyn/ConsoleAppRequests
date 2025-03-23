using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/weatherforecast")]
    public class WeatherforecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> GetData()
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var forecast = Enumerable.Range(1, 5).Select(index =>
                            new WeatherForecast
                            {
                                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                TemperatureF = Random.Shared.Next(-20, 55),
                                Summary = summaries[Random.Shared.Next(summaries.Length)]
                            })
                        .ToArray();
            return forecast;
        }


        [Authorize]
        [Route("secured")]
        [HttpGet]
        public IEnumerable<WeatherForecast> GetDataSecured()
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var forecast = Enumerable.Range(1, 5).Select(index =>
                            new WeatherForecast
                            {
                                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                TemperatureF = Random.Shared.Next(-20, 55),
                                Summary = summaries[Random.Shared.Next(summaries.Length)]
                            })
                        .ToArray();
            return forecast;
        }
    }
}