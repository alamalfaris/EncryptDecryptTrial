using EncryptDecryptTrial.Helpers;
using EncryptDecryptTrial.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EncryptDecryptTrial.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IAesService _aesService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, 
			IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
			_aesService = new AesService();
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			var connStringAesDecrypted = _aesService.DecryptAsync(Convert.FromBase64String(_configuration.GetSection("ConnectionString").Value)).Result;

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}