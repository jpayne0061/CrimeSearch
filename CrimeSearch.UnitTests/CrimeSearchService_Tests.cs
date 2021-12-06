using CrimeSearch.Models;
using CrimeSearch.Services;
using CrimeSearch.Statics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CrimeSearch.UnitTests
{

    public class CrimeSearchService_Tests
    {
        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void Test1()
        {
            List<CrimeInstance> crimes = new List<CrimeInstance> { 
                new CrimeInstance { ZIP_CODE = "40208", DATE_REPORTED = new DateTime(2021, 1, 1) }, 
                new CrimeInstance { ZIP_CODE = "40203", DATE_REPORTED = new DateTime(2021, 3, 14)} 
            };

            List<PredicateOperation> predicateOperations = new List<PredicateOperation>
            {
                new PredicateOperation { 
                    Delegate = ComparingDelegates.Equals,
                    FieldName = "ZIP_CODE",
                    Value = "40203",
                    Operator = "="
                }
            };

            FuncBuilder funcBuilder = new FuncBuilder();
            Expression<Func<CrimeInstance, bool>> exp = funcBuilder.BuildFunc(predicateOperations);

            var c = crimes.Where(exp).ToList();

            Assert.Pass();
        }
    }
}