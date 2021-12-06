using CrimeSearch.Models;
using System.Collections.Generic;

namespace CrimeSearch.Interfaces
{
    public interface IPredicateOperationBuilder
    {
        List<PredicateOperation> BuildPredicateOperations(IEnumerable<string> predicates);
    }
}
