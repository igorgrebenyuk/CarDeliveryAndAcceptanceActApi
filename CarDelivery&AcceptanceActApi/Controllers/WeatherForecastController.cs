using Microsoft.AspNetCore.Mvc;

namespace CarDelivery_AcceptanceActApi.Controllers
{

    /// <summary>
    /// Прогноз погоды
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];


        /// <summary>
        /// Возращает прогноз погоды 
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
        var random = Random.Shared.Next() % 2 == 1;
            if (random)
            { 
                return BadRequest();
            }
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
