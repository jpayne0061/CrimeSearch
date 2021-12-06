using System;

namespace CrimeSearch.Models
{
    public class PredicateOperation
    {
        public Func<IComparable, object, bool> Delegate { get; set; }
        public string FieldName { get; set; }
        public object Value { get; set; }
        public string Operator { get; set; }
    }
}
