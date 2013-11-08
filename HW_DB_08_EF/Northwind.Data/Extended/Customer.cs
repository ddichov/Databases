using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Data
{
    public partial class Customer
    {
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}.", this.CustomerID, this.CompanyName,  
                this.City, this.ContactName, this.ContactTitle, this.Country);
        }
    }
}
