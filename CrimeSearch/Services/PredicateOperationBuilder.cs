using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CrimeSearch.Services
{
    public class PredicateOperationBuilder : IPredicateOperationBuilder
    {
        public List<PredicateOperation> BuildPredicateOperations(IEnumerable<SearchParameter> predicates)
        {
            var operatorToDelegate = new Dictionary<string, ExpressionType>
            {
                { ">",         ExpressionType.GreaterThan },
                { "<",         ExpressionType.LessThan},
                { "=",         ExpressionType.Equal},
                { ">=",        ExpressionType.GreaterThanOrEqual},
                { "<=",        ExpressionType.LessThanOrEqual},
                { "!=",        ExpressionType.NotEqual}
            };

            var conjunctiveOperatorMap = new Dictionary<string, AndOr>
            {
                { "and", AndOr.And },
                { "or", AndOr.Or }
            };

            return predicates.Select(x => new PredicateOperation
                {
                    ExpressionType = operatorToDelegate[x.SearchOperator],
                    Value = ConvertToType(x.FieldType, x.SearchValue, x.FieldName),
                    FieldName = x.FieldName
            }).ToList();
        }

        public List<PredicateOperation> BuildPredicateOperationsFromQuery(string query)
        {
            var predicates = ParseQuery(new List<PredicateOperation>(), query, 0);

            return predicates;
        }

      
        public List<PredicateOperation> ParseQuery(List<PredicateOperation> predicateOperations, string query, int expressionStart)
        {
            if(expressionStart > query.Length - 6)
            {
                return predicateOperations;
            }

            string fieldName = string.Empty;

            ExpressionType expressionType = ExpressionType.IsTrue;

            int fieldIndexStart = 0;

            if (query.Substring(expressionStart, 2) == "or")
            {
                expressionType = ExpressionType.Or;
                fieldIndexStart = expressionStart + 3;
            }
            else if(query.Substring(expressionStart, 3) == "and")
            {
                expressionType = ExpressionType.And;
                fieldIndexStart = expressionStart + 4;
            }

            int operatorIndexStart = 0;

            List<char> fieldNameChars = new List<char>();

            for (int i = fieldIndexStart; i < query.Length; i++)
            {
                if(query[i] == ']')
                {
                    operatorIndexStart = i + 1;
                    break;
                }

                if(query[i] == '[')
                {
                    continue;
                }

                fieldNameChars.Add(query[i]);
            }

            fieldName = new string(fieldNameChars.ToArray());

            int constantStart = 0;

            List<char> unParsedOperator = new List<char>();


            for (int i = operatorIndexStart; i < query.Length; i++)
            {
                if(query[i] == '\'')
                {
                    constantStart = i + 1;
                    break;
                }

                unParsedOperator.Add(query[i]);
            }

            string parsedOperator = new string(unParsedOperator.ToArray());

            int expressionIndexEnd = 0;

            List<char> fieldChars = new List<char>();

            for (int i = constantStart; i < query.Length; i++)
            {
                if (query[i] == '\'')
                {
                    expressionIndexEnd = i;
                    break;
                }

                fieldChars.Add(query[i]);
            }

            string constant = new string(query.Skip(constantStart).TakeWhile(x => x != '\'').ToArray());

            predicateOperations.Add(new PredicateOperation { ExpressionType = expressionType, FieldName = fieldName, Value = constant });

            return ParseQuery(predicateOperations, query, expressionIndexEnd + 1);
        }

        private IComparable ConvertToType(string fieldType, string val, string fieldName)
        {
            try
            {
                IComparable convertedVal;

                switch (fieldType)
                {
                    case "bool":
                        convertedVal = Convert.ToBoolean(val);
                        break;
                    case "decimal":
                        convertedVal = Convert.ToDecimal(val);
                        break;
                    case "int32":
                        convertedVal = Convert.ToInt32(val);
                        break;
                    case "int64":
                        convertedVal = Convert.ToInt64(val);
                        break;
                    case "string":
                        convertedVal = val;
                        break;
                    case "datetime":
                        convertedVal = Convert.ToDateTime(val);
                        break;
                    default:
                        convertedVal = null;
                        break;
                }

                return convertedVal;
            }
            catch (FormatException ex)
            {
                string message = $"Failed while parsing value. Field name: ${fieldName}. Field type: ${fieldType}. Value: ${val}.";

                throw new Exception(message, ex);
            }
        }
    }
}
