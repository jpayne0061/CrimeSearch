using CrimeSearch.Models;
using CrimeSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
        public async Task<object> GetTopFive()
        {
            object crimeInstances = await _crimeSearch.GetTopFive();

            return crimeInstances;
        }

        [HttpPost]
        public async Task<object> Get([FromBody] List<SearchParameter> searchParameters)
        {
            object crimeInstances = await _crimeSearch.GetByParameters(searchParameters);

            return crimeInstances;
        }

        //[HttpGet]
        //public async Task<IEnumerable<CrimeInstance>> Get(string args)
        //{
        //    //parse args


        //    //create expession

        //    //pass expression to repo?



        //    IEnumerable<CrimeInstance> crimeInstances = await _crimeSearch.GetByParameters(searchParameters);

        //    return crimeInstances;
        //}
    }
}
