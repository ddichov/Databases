using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.SQLite.Models
{
    public class TaxTable
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Tax { get; set; }
    }
}
