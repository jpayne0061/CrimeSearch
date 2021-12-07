using CrimeSearch.Models;
using CrimeSearch.Services;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CrimeSearch.UnitTests
{

    public class CrimeSearchService_Tests
    {
        [Test]
        public void ExpressionBuilder_Filter_String_Success()
        {
            //arrange
            List<CrimeInstance> crimes = new List<CrimeInstance> { 
                new CrimeInstance { ZIP_CODE = "40208", DATE_REPORTED = new DateTime(2021, 1, 1) }, 
                new CrimeInstance { ZIP_CODE = "40203", DATE_REPORTED = new DateTime(2021, 3, 14)} 
            };

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation { 
                    ExpressionType = ExpressionType.Equal,
                    FieldName = "ZIP_CODE",
                    Value = "40203"
                }
            };

            ExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<CrimeInstance, bool>> exp = expressionBuilder.BuildExpression<CrimeInstance>(predicateOperations);

            //act
            var crimeResult = crimes.AsQueryable().Where(exp).ToList();

            //assert
            Assert.AreEqual(1, crimeResult.Count());
            Assert.AreEqual(new DateTime(2021,3,14), crimeResult[0].DATE_REPORTED);
            Assert.AreEqual("40203", crimeResult[0].ZIP_CODE);
        }

        [Test]
        public void ExpressionBuilder_Filter_Many_Success()
        {
            //arrange
            List<CrimeInstance> crimes = new List<CrimeInstance> {
                new CrimeInstance { CITY = "a", DATE_REPORTED = new DateTime(2021, 1, 1) },
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "c", DATE_REPORTED = new DateTime(2021, 3, 14)}
            };

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation {
                    ExpressionType = ExpressionType.Equal,
                    FieldName = "CITY",
                    Value = "b"
                }
            };

            ExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<CrimeInstance, bool>> exp = expressionBuilder.BuildExpression<CrimeInstance>(predicateOperations);

            //act
            var crimeResult = crimes.AsQueryable().Where(exp).ToList();

            //assert
            Assert.AreEqual(2, crimeResult.Count());
            Assert.AreEqual("b", crimeResult[0].CITY);
            Assert.AreEqual("b", crimeResult[1].CITY);
        }

        [Test]
        public void ExpressionBuilder_Filter_Date_Success()
        {
            //arrange
            List<CrimeInstance> crimes = new List<CrimeInstance> {
                new CrimeInstance { CITY = "a", DATE_REPORTED = new DateTime(2021, 1, 1) },
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "c", DATE_REPORTED = new DateTime(2021, 3, 14)}
            };

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation {
                    ExpressionType = ExpressionType.Equal,
                    FieldName = "DATE_REPORTED",
                    Value = new DateTime(2021, 3, 14)
                }
            };

            ExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<CrimeInstance, bool>> exp = expressionBuilder.BuildExpression<CrimeInstance>(predicateOperations);

            //act
            var crimeResult = crimes.AsQueryable().Where(exp).ToList();

            //assert
            Assert.AreEqual(3, crimeResult.Count());
            Assert.AreEqual(new DateTime(2021, 3, 14), crimeResult[0].DATE_REPORTED);
            Assert.AreEqual(new DateTime(2021, 3, 14), crimeResult[1].DATE_REPORTED);
            Assert.AreEqual(new DateTime(2021, 3, 14), crimeResult[1].DATE_REPORTED);
        }

        [Test]
        public void ExpressionBuilder_Filter_With_Multiple_Predicates_Success()
        {
            //arrange
            List<CrimeInstance> crimes = new List<CrimeInstance> {
                new CrimeInstance { CITY = "a", DATE_REPORTED = new DateTime(2021, 1, 1) },
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "b", DATE_REPORTED = new DateTime(2021, 3, 14)},
                new CrimeInstance { CITY = "c", DATE_REPORTED = new DateTime(2021, 3, 14)}
            };

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation {
                    ExpressionType = ExpressionType.Equal,
                    FieldName = "DATE_REPORTED",
                    Value = new DateTime(2021, 3, 14)
                },
                new PredicateOperation {
                    ExpressionType = ExpressionType.Equal,
                    FieldName = "CITY",
                    Value = "c"
                }
            };

            ExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<CrimeInstance, bool>> exp = expressionBuilder.BuildExpression<CrimeInstance>(predicateOperations);

            //act
            var crimeResult = crimes.AsQueryable().Where(exp).ToList();

            //assert
            Assert.AreEqual(1, crimeResult.Count());
            Assert.AreEqual(new DateTime(2021, 3, 14), crimeResult[0].DATE_REPORTED);
            Assert.AreEqual("c", crimeResult[0].CITY);
        }
    }
}