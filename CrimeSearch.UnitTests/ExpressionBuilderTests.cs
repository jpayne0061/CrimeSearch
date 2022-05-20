using CrimeSearch.Models;
using CrimeSearch.Services;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace CrimeSearch.UnitTests
{

    public class ExpressionBuilderTests
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

        [Test]
        public void ExpressionBuilder_Filter_With_Multiple_Predicates_Using_Dyanamic_Success()
        {
            string json = "[{ \"Name\": \"Bob\", \"Age\": 37 }, {\"Name\": \"Steven\", \"Age\": 45}, {\"Name\": \"Joe\", \"Age\": 99}]";

            List<JObject> dynamics = JsonConvert.DeserializeObject<List<JObject>>(json);

            string query = "[Name]='Bob'or[Age]>'98'";

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation{ ExpressionType = ExpressionType.Equal, AndOr = null, FieldName = "Name", Value="Bob"},
                new PredicateOperation{ ExpressionType = ExpressionType.GreaterThan, AndOr = ExpressionType.Or, FieldName = "Age", Value = 98L }
            };
            //new PredicateOperationBuilder().BuildPredicateOperationsFromQuery(query);

            ExpressionBuilder expressionBuilder = new ExpressionBuilder();
            Expression<Func<object, bool>> exp = expressionBuilder.BuildExpressionForDynamicObject_testObject<object>(predicateOperations);

            //act
            //var crimeResult = crimes.AsQueryable().Where(exp).ToList();

            var result = dynamics.AsQueryable().Where(exp).ToList();

            ////assert
            //Assert.AreEqual(1, crimeResult.Count());
            //Assert.AreEqual(new DateTime(2021, 3, 14), crimeResult[0].DATE_REPORTED);
            //Assert.AreEqual("c", crimeResult[0].CITY);
        }


    }
}