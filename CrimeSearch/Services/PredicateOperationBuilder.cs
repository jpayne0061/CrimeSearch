using CrimeSearch.Interfaces;
using CrimeSearch.Models;
using CrimeSearch.Statics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrimeSearch.Services
{
    public class PredicateOperationBuilder : IPredicateOperationBuilder
    {

        /// <summary>
        /// Parses a predicate string that looks something like this: "FIELD_NAME > FIELD_VALUE FIELD_TYPE".
        /// </summary>
        /// <param name="predicates">A string representation of the predicate operation</param>
        /// <returns>Returns a predicate operation object that can be used to build mongodb queries</returns>
        public List<PredicateOperation> BuildPredicateOperations(IEnumerable<string> predicates)
        {
            var operatorToDelegate = new Dictionary<string, Func<IComparable, object, bool>>
            {
                { ">",         ComparingDelegates.IsMoreThan },
                { "<",         ComparingDelegates.IsLessThan},
                { "=",         ComparingDelegates.IsEqualTo},
                { ">=",        ComparingDelegates.MoreThanOrEqualTo},
                { "<=",        ComparingDelegates.LessThanOrEqualTo},
                { "!=",        ComparingDelegates.NotEqualTo},
                { "contains",  ComparingDelegates.Contains},
            };

            var predicateOperations = new List<PredicateOperation>();

            foreach (var predicate in predicates)
            {
                //string predicate representation
                //FIELD_NAME operator FIELD_VALUE FIELD_TYPE
                //    0          1        2           3  

                List<string> predicateOperatorParts = predicate.Split(' ').ToList();

                
                string fieldName  =  predicateOperatorParts[0]; 
                string operation  =  predicateOperatorParts[1];
                string fieldValue =  predicateOperatorParts[2];
                string fieldType  =  predicateOperatorParts[3];

                var predicateOperation       = new PredicateOperation();
                predicateOperation.Delegate  = operatorToDelegate[operation];
                predicateOperation.Operator  = operation;
                predicateOperation.Value     = ConvertToType(fieldType, fieldValue, fieldName);
                predicateOperation.FieldName = fieldName;

                predicateOperations.Add(predicateOperation);
            }

            return predicateOperations;
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
