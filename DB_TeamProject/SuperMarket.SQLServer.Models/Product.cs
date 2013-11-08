using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.SQLServer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public string ProductName { get; set; }
        public int MeasureId { get; set; }
        public virtual Measure Measure { get; set; }
        public decimal BasePrice { get; set; }
    }
}
