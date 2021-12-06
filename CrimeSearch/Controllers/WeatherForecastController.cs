using CrimeSearch.Models;
using CrimeSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrimeSearch.Controllers
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
        private readonly CrimeSearchService _crimeSearch;

        public WeatherForecastController(CrimeSearchService crimeSearch)
        {
            _crimeSearch = crimeSearch;
        }

        [HttpPost]
        public async Task<IEnumerable<CrimeInstance>> Get([FromBody] PredicateOperation searchParameters)
        {
            //accept crime instance object

            //for every non-null or non-default property, add to mongodb search parameters


            IEnumerable<CrimeInstance> crimeInstances = null;

            if (searchParameters.FieldName == "ZIP_CODE")
            {
                crimeInstances = await _crimeSearch.GetAsyncByZipCode(searchParameters.FieldValue);
            }

            return crimeInstances;
        }
    }
}
