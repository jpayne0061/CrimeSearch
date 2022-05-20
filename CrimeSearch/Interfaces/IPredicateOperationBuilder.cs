using CrimeSearch.Models;
using System.Collections.Generic;

namespace CrimeSearch.Interfaces
{
    public interface IPredicateOperationBuilder
    {
        List<PredicateOperation> BuildPredicateOperations(IEnumerable<SearchParameter> predicates);
        List<PredicateOperation> BuildPredicateOperationsFromQuery(string query);
    }
}
