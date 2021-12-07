using CrimeSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CrimeSearch.Services
{
    public class ExpressionBuilder
    {
        public Expression<Func<T, bool>> BuildExpression<T>(List<PredicateOperation> predicateOperations)
        {
            BinaryExpression finalExpression = null;

            ParameterExpression argParam = Expression.Parameter(typeof(T), "x");

            foreach (var predicate in predicateOperations)
            {
                Expression nameProperty = Expression.Property(argParam, predicate.FieldName);

                ConstantExpression valueToCompare = Expression.Constant(predicate.Value);

                BinaryExpression binaryExpression = Expression.MakeBinary(predicate.ExpressionType, nameProperty, valueToCompare);

                if(finalExpression == null)
                    finalExpression = binaryExpression;
                else
                    finalExpression = Expression.MakeBinary(ExpressionType.And, finalExpression, binaryExpression);
            }

            return Expression.Lambda<Func<T, bool>>(finalExpression, argParam);
        }

    }
}
