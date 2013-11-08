using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Formats
{
    public class SqlPdfFormat
    {
        public DateTime SoldDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Measure { get; set; }
        public decimal UnitPrice { get; set; }
        public string SupermarketName { get; set; }
        public decimal Sum { get; set; }
    }
}
