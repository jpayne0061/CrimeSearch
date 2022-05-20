using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrimeSearch.Services
{
    public class CrimeSearchService
    {
        private readonly IMongoCollection<JObject> _crimeCollection;
        private readonly IMongoCollection<Dictionary<string, object>> _testCollection;
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Dictionary<string, object>> _dynoCollection;
        private readonly IPredicateOperationBuilder      _predicateOperationBuilder;
        private readonly ExpressionBuilder               _expressionBuilder;

        public CrimeSearchService(IPredicateOperationBuilder predicateOperationBuilder, 
            ExpressionBuilder funcBuilder, IMongoCollection<JObject> crimeCollection, 
            IMongoCollection<Dictionary<string, object>> testCollection, IMongoDatabase mongoDatabase)
        {
            _crimeCollection =           crimeCollection;
            _testCollection = testCollection;
            _mongoDatabase = mongoDatabase;
            _predicateOperationBuilder = predicateOperationBuilder;
            _expressionBuilder =         funcBuilder;
        }

        public async Task<object> GetByParameters(List<SearchParameter> predicates)
        {
            _dynoCollection = _mongoDatabase.GetCollection<Dictionary<string, object>>("CrimeInstance");

            List<PredicateOperation> predicateOperations = _predicateOperationBuilder.BuildPredicateOperations(predicates);

            Expression<Func<Dictionary<string, object>, bool>> expression = _expressionBuilder.BuildExpressionForDynamicObject_testObject<object>(predicateOperations);

            var crimes = _testCollection.Find(expression).Limit(1000).ToList();

            return crimes;
        }

        public async Task<object> GetTopFive()
        {
            FindOptions findOptions = new FindOptions();
            findOptions.ShowRecordId = false;

            string json = "";

            var crimes = _testCollection.Find(x => true).Limit(5).ToList();

            return crimes;
        }

    }
}
