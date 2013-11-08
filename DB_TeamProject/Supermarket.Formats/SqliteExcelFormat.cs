using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Formats
{
    public class SqliteExcelFormat
    {
        public string Vendor { get; set; }
        public int ProductId { get; set; }
        public decimal? Incomes { get; set; }
        public decimal? Expenses { get; set; }
        public decimal? TaxPercentage { get; set; }
        
    }
}
