using DockerAspNetDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace DockerAspNetDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IMessageProducer _producer;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessageProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("/rabbit")]
        public ActionResult SaveAnswer()
        {
            var message = new
            {
                Answer = "Answer",
                IsCorrect = true,
                QuizItemId = 2

            };
            _producer.SendMessage(message);
            return Ok(); 
        }
    }
}