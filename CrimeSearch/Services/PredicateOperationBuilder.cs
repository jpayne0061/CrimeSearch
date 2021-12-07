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

            return predicates.Select(x => new PredicateOperation
                {
                    ExpressionType = operatorToDelegate[x.SearchOperator],
                    Value = ConvertToType(x.FieldType, x.SearchValue, x.FieldName),
                    FieldName = x.FieldName
                }).ToList();
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
