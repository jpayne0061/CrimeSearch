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
    public class DynamicSearchController : ControllerBase
    {
        private readonly DynamicSearchService _dynamicSearch;

        public DynamicSearchController(DynamicSearchService dynamicSearch)
        {
            _dynamicSearch = dynamicSearch;
        }

        [HttpGet]
        public async Task<IEnumerable<dynamic>> GetTopFive()
        {
            IEnumerable<dynamic> documents = await _dynamicSearch.GetTopFive();

            return documents;
        }

        //[HttpGet]
        //public async Task<IEnumerable<CrimeInstance>> Get(string args)
        //{
        //    //parse args


        //    //create expession

        //    //pass expression to repo?



        //    IEnumerable<CrimeInstance> crimeInstances = await _dynamicSearch.GetByParameters(searchParameters);

        //    return crimeInstances;
        //}
    }
}
