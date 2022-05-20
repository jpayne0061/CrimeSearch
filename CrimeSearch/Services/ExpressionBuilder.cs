using CrimeSearch.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
                    finalExpression = Expression.MakeBinary(predicate.ExpressionType, finalExpression, binaryExpression);
            }

            return Expression.Lambda<Func<T, bool>>(finalExpression, argParam);
        }

        public Expression<Func<JObject, bool>> BuildExpressionForDynamicObject_testJObject<dynamic>(List<PredicateOperation> predicateOperations)
        {
            Expression<Func<JObject, bool>> finalExpression = null;

            ParameterExpression argParam = Expression.Parameter(typeof(JObject), "x");

            foreach (PredicateOperation predicate in predicateOperations)
            {
                if (predicate.ExpressionType == ExpressionType.GreaterThan)
                {
                    Expression<Func<JObject, bool>> expr2 = item => ((IComparable)((JValue)item.Property(predicate.FieldName).Value).Value).CompareTo((IComparable)predicate.Value) == 1;

                    if (finalExpression == null)
                    {
                        finalExpression = expr2;
                    }
                    else if (predicate.AndOr == ExpressionType.And)
                    {
                        finalExpression = finalExpression.And(expr2);
                    }
                    else if (predicate.AndOr == ExpressionType.Or)
                    {
                        finalExpression = finalExpression.Or(expr2);
                    }
                }
                else if (predicate.ExpressionType == ExpressionType.Equal)
                {
                    Expression<Func<JObject, bool>> expr2 = item => ((IComparable)((JValue)item.Property(predicate.FieldName).Value).Value).CompareTo((IComparable)predicate.Value) == 0;

                    if (finalExpression == null)
                    {
                        finalExpression = expr2;
                    }
                    else if (predicate.ExpressionType == ExpressionType.And)
                    {
                        finalExpression = finalExpression.And(expr2);
                    }
                    else if (predicate.ExpressionType == ExpressionType.Or)
                    {
                        finalExpression = finalExpression.Or(expr2);
                    }
                }
                else if (predicate.ExpressionType == ExpressionType.LessThan)
                {
                    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value < (int)predicate.Value;

                    finalExpression = expr2;
                }

                else if (predicate.ExpressionType == ExpressionType.GreaterThanOrEqual)
                {
                    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value >= (int)predicate.Value;

                    finalExpression = expr2;
                }
                else if (predicate.ExpressionType == ExpressionType.LessThanOrEqual)
                {
                    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value <= (int)predicate.Value;

                    finalExpression = expr2;
                }
                else if (predicate.ExpressionType == ExpressionType.NotEqual)
                {
                    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value != (int)predicate.Value;

                    finalExpression = expr2;
                }
            }

            return finalExpression;
        }

        public Expression<Func<Dictionary<string, object>, bool>> BuildExpressionForDynamicObject_testObject<dynamic>(List<PredicateOperation> predicateOperations)
        {
            Expression<Func<Dictionary<string, object>, bool>> finalExpression = null;

            ParameterExpression argParam = Expression.Parameter(typeof(object), "x");

            foreach (PredicateOperation predicate in predicateOperations)
            {
                if (predicate.ExpressionType == ExpressionType.GreaterThan)
                {
                    Expression<Func<Dictionary<string, object>, bool>> expr2 = item => ((IComparable)item[predicate.FieldName.ToUpper()]).CompareTo((IComparable)predicate.Value) == 1;

                    if (finalExpression == null)
                    {
                        finalExpression = expr2;
                    }
                    else if (predicate.AndOr == ExpressionType.And)
                    {
                        finalExpression = finalExpression.And(expr2);
                    }
                    else if (predicate.AndOr == ExpressionType.Or)
                    {
                        finalExpression = finalExpression.Or(expr2);
                    }
                }
                else if (predicate.ExpressionType == ExpressionType.Equal)
                {
                    Expression<Func<Dictionary<string, object>, bool>> expr2 = item => ((IComparable)item[predicate.FieldName.ToUpper()]).CompareTo((IComparable)predicate.Value) == 0;

                    if (finalExpression == null)
                    {
                        finalExpression = expr2;
                    }
                    else if (predicate.ExpressionType == ExpressionType.And)
                    {
                        finalExpression = finalExpression.And(expr2);
                    }
                    else if (predicate.ExpressionType == ExpressionType.Or)
                    {
                        finalExpression = finalExpression.Or(expr2);
                    }
                }
                //else if (predicate.ExpressionType == ExpressionType.LessThan)
                //{
                //    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value < (int)predicate.Value;

                //    finalExpression = expr2;
                //}

                //else if (predicate.ExpressionType == ExpressionType.GreaterThanOrEqual)
                //{
                //    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value >= (int)predicate.Value;

                //    finalExpression = expr2;
                //}
                //else if (predicate.ExpressionType == ExpressionType.LessThanOrEqual)
                //{
                //    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value <= (int)predicate.Value;

                //    finalExpression = expr2;
                //}
                //else if (predicate.ExpressionType == ExpressionType.NotEqual)
                //{
                //    Expression<Func<JObject, bool>> expr2 = item => (int)((JValue)item.Property(predicate.FieldName).Value).Value != (int)predicate.Value;

                //    finalExpression = expr2;
                //}
            }

            return finalExpression;
        }

        public Expression<Func<dynamic, bool>> BuildExpressionForDynamicObject<dynamic>(List<PredicateOperation> predicateOperations, JObject obj)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();

            BinaryExpression finalExpression = null;

            ParameterExpression argParam = Expression.Parameter(typeof(dynamic), "x");

            foreach (var predicate in predicateOperations)
            {
                PropertyInfo pi = propertyInfos.Where(x => x.Name == predicate.FieldName).FirstOrDefault();

                Expression nameProperty = Expression.Property(argParam, pi);

                ConstantExpression valueToCompare = Expression.Constant(predicate.Value);

                BinaryExpression binaryExpression = Expression.MakeBinary(predicate.ExpressionType, nameProperty, valueToCompare);

                if (finalExpression == null)
                    finalExpression = binaryExpression;
                else
                    finalExpression = Expression.MakeBinary(predicate.ExpressionType, finalExpression, binaryExpression);
            }

            return Expression.Lambda<Func<dynamic, bool>>(finalExpression, argParam);
        }

        private IQueryable<T> AddOrderBy<T>(IQueryable<T> query, Expression<Func<T, object>> orderByProperty, bool isAscending, bool isFirst)
        {
            Expression<Func<IOrderedQueryable<int>, IQueryable<int>>> methodDef = isAscending
                ? (isFirst ? (Expression<Func<IOrderedQueryable<int>, IQueryable<int>>>)(q => q.OrderBy(x => x)) : (Expression<Func<IOrderedQueryable<int>, IQueryable<int>>>)(q => q.ThenBy(x => x)))
                : (isFirst ? (Expression<Func<IOrderedQueryable<int>, IQueryable<int>>>)(q => q.OrderByDescending(x => x)) : (Expression<Func<IOrderedQueryable<int>, IQueryable<int>>>)(q => q.ThenByDescending(x => x)));

            // get the property type
            var propExpression = orderByProperty.Body.NodeType == ExpressionType.Convert && orderByProperty.Body.Type == typeof(object)
                ? (LambdaExpression)Expression.Lambda(((UnaryExpression)orderByProperty.Body).Operand, orderByProperty.Parameters)
                : orderByProperty;

            var methodInfo = ((MethodCallExpression)methodDef.Body).Method.GetGenericMethodDefinition().MakeGenericMethod(typeof(T), propExpression.Body.Type);
            return (IQueryable<T>)methodInfo.Invoke(null, new object[] { query, propExpression });
        }

    }

    public static class PredicateBuilder
    {
        // Creates a predicate that evaluates to true.        
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        // Creates a predicate that evaluates to false.        
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        // Creates a predicate expression from the specified lambda expression.        
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        // Combines the first predicate with the second using the logical "and".        
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        // Combines the first predicate with the second using the logical "or".        
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        // Negates the predicate.        
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        // Combines the first expression with the second using the specified merge function.        
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
