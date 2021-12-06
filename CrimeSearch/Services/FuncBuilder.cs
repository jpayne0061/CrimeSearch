using CrimeSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrimeSearch.Services
{
    public class FuncBuilder
    {
        /// <summary>
        /// The core of this method was stoeln from Jon Skeet. 
        /// https://stackoverflow.com/questions/5094489/how-do-i-dynamically-create-an-expressionfuncmyclass-bool-predicate-from-ex
        /// </summary>
        /// <returns></returns>
        public Expression<Func<CrimeInstance, bool>> BuildFunc(IEnumerable<PredicateOperation> predicateOperations)
        {
            Expression expression = null;

            ParameterExpression argParam = Expression.Parameter(typeof(CrimeInstance), "x");

            foreach(PredicateOperation predicateOperation in predicateOperations)
            {
                Expression nameProperty = Expression.Property(argParam, predicateOperation.FieldName);

                ConstantExpression valueToCompare = Expression.Constant(predicateOperation.Value);

                Expression e1 = Expression.Equal(nameProperty, valueToCompare);

                var andExp = Expression.And(nameProperty, valueToCompare);

                expression = andExp;
            }

            return Expression.Lambda<Func<CrimeInstance, bool>>(expression, argParam);
        }
    }
}
