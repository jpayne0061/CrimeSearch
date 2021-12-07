using CrimeSearch.Models;
using CrimeSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrimeSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeSearchController : ControllerBase
    {
        private readonly CrimeSearchService _crimeSearch;

        public CrimeSearchController(CrimeSearchService crimeSearch)
        {
            _crimeSearch = crimeSearch;
        }

        [HttpGet]
        public async Task<IEnumerable<CrimeInstance>> GetTopFive()
        {
            IEnumerable<CrimeInstance> crimeInstances = await _crimeSearch.GetTopFive();

            return crimeInstances;
        }

        [HttpPost]
        public async Task<IEnumerable<CrimeInstance>> Get([FromBody] List<SearchParameter> searchParameters)
        {
            IEnumerable<CrimeInstance> crimeInstances = await _crimeSearch.GetByParameters(searchParameters);

            return crimeInstances;
        }
    }
}
