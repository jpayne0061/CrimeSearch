using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CrimeSearch.Models
{
    public class CrimeInstance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string INCIDENT_NUMBER { get; set; }
        public DateTime DATE_REPORTED { get; set; }
        public DateTime DATE_OCCURED { get; set; }
        public string UOR_DESC { get; set; }
        public string CRIME_TYPE { get; set; }
        public string NIBRS_CODE { get; set; }
        public string UCR_HIERARCHY { get; set; }
        public string ATT_COMP { get; set; }
        public string LMPD_DIVISION { get; set; }
        public string LMPD_BEAT { get; set; }
        public string PREMISE_TYPE { get; set; }
        public string BLOCK_ADDRESS { get; set; }
        public string CITY { get; set; }
        public string ZIP_CODE { get; set; }
    }
}
