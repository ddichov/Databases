using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.SQLServer.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public DateTime Month { get; set; }
        public decimal ExpenseValue { get; set; }
    }
}
