namespace CrimeSearch.Models
{
    public class SearchParameter
    {
        public string FieldName { get; set; }
        public string SearchOperator { get; set; }
        public string SearchValue { get; set; }
        public string FieldType { get; set; }
        public string ConjunctiveOperator { get; set; }
    }
}
