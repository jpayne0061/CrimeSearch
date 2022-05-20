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
    public class PredicateOperationBuilderTests
    {
        [Test]
        public void PredicateOperationBuilder_ParseNextExpression()
        {
            //arrange
            string query = "[fieldName]>'6'or[anotherField]<='whoa'";

            PredicateOperationBuilder predicateOperationBuilder = new PredicateOperationBuilder();

            //act
            List<PredicateOperation> predicateOperations = predicateOperationBuilder.ParseQuery(new List<PredicateOperation>(), query, 0);

            //assert
        }
    }
}
