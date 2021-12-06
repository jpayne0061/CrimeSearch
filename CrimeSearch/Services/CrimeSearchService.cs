using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrimeSearch.Services
{
    public class CrimeSearchService
    {
        private readonly IMongoCollection<CrimeInstance> _crimeCollection;
        private readonly PredicateOperationBuilder predicateOperationBuilder;

        public CrimeSearchService(DBSettings dbSettings, IPredicateOperationBuilder predicateOperationBuilder)
        {
            var mongoClient = new MongoClient(
                dbSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dbSettings.DatabaseName);

            _crimeCollection = mongoDatabase.GetCollection<CrimeInstance>(
                dbSettings.CollectionName);
        }

        public async Task<List<CrimeInstance>> GetAsync() =>
            await _crimeCollection.Find(_ => true).ToListAsync();

        public async Task<CrimeInstance?> GetAsync(string id) =>
            await _crimeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<CrimeInstance>> GetAsyncByZipCode(string zipCode) =>
                await _crimeCollection.Find(x => x.ZIP_CODE == zipCode).ToListAsync();

        public async Task<List<CrimeInstance>> GetByParameters(List<string> predicates)
        {
            List<PredicateOperation> predicateOperations = predicateOperationBuilder.BuildPredicateOperations(predicates);

            _crimeCollection.Find()

            _crimeCollection.Find(x => x.DATE_REPORTED > new DateTime()).Filter

        }

    }
}
