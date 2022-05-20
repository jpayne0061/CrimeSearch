using System;
using System.Linq.Expressions;

namespace CrimeSearch.Models
{
    public class PredicateOperation
    {
        public ExpressionType ExpressionType { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
        public ExpressionType? AndOr { get; set; }
    }
}
