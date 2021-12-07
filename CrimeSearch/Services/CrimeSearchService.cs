using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrimeSearch.Services
{
    public class CrimeSearchService
    {
        private readonly IMongoCollection<CrimeInstance> _crimeCollection;
        private readonly IPredicateOperationBuilder      _predicateOperationBuilder;
        private readonly ExpressionBuilder               _expressionBuilder;

        public CrimeSearchService(IPredicateOperationBuilder predicateOperationBuilder, ExpressionBuilder funcBuilder, IMongoCollection<CrimeInstance> crimeCollection)
        {
            _crimeCollection =           crimeCollection;
            _predicateOperationBuilder = predicateOperationBuilder;
            _expressionBuilder =         funcBuilder;
        }

        public async Task<List<CrimeInstance>> GetByParameters(List<SearchParameter> predicates)
        {
            List<PredicateOperation> predicateOperations = _predicateOperationBuilder.BuildPredicateOperations(predicates);

            Expression<Func<CrimeInstance, bool>> expression = _expressionBuilder.BuildExpression<CrimeInstance>(predicateOperations);

            var crimes =  await _crimeCollection.Find(expression).Limit(1000).ToListAsync();

            return crimes;
        }

        public async Task<List<CrimeInstance>> GetTopFive()
        {
            var crimes = await _crimeCollection.Find(x => true).Limit(5).ToListAsync();

            return crimes;
        }

    }
}
