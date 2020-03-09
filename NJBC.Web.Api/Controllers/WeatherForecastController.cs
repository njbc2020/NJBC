using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;

namespace NJBC.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {



        private readonly ISemEvalRepository SemEvalRepository;

        private static readonly string[] Summaries = new[]
        {

            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISemEvalRepository SemEvalRepository)
        {
            _logger = logger;
            this.SemEvalRepository = SemEvalRepository;

        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await SemEvalRepository.AddOrgQuestion(new OrgQuestion()
            {
                OrgQbody = "string",
                OrgqIdName = "name",
                OrgQsubject = "obj",

            }, true);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}