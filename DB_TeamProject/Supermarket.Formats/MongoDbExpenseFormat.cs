using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SuperMarket.Formats
{
    public class MongoDbExpenseFormat
    {
        public ObjectId Id { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime Month { get; set; }
        public decimal Expense { get; set; }
    }
}
