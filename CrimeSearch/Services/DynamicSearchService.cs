using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrimeSearch.Services
{
    public class DynamicSearchService
    {
        private readonly IMongoCollection<dynamic> _collection;
        private readonly IPredicateOperationBuilder _predicateOperationBuilder;
        private readonly ExpressionBuilder _expressionBuilder;

        public DynamicSearchService(IPredicateOperationBuilder predicateOperationBuilder, ExpressionBuilder funcBuilder, IMongoDatabase mongoDatabase, string collectionName)
        {
            _collection = mongoDatabase.GetCollection<dynamic>(collectionName);
            _predicateOperationBuilder = predicateOperationBuilder;
            _expressionBuilder = funcBuilder;
        }

        public async Task<List<dynamic>> GetByQuery(string query)
        {
            List<PredicateOperation> predicateOperations = _predicateOperationBuilder.BuildPredicateOperationsFromQuery(query);

            Expression<Func<dynamic, bool>> expression = _expressionBuilder.BuildExpression<dynamic>(predicateOperations);

            var documents = await _collection.Find(expression).Limit(1000).ToListAsync();

            return documents;
        }

        public async Task<List<dynamic>> GetTopFive()
        {
            var documents = await _collection.Find(x => true).Limit(5).ToListAsync();

            return documents;
        }
    }
}
