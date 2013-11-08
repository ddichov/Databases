using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Formats
{
    public class SqlXmlFormat
    {
        public string VendorName { get; set; }
        public DateTime SoldDate { get; set; }
        public decimal Sum { get; set; }
    }
}
