using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.MySQL.Data;

namespace SuperMarket.SQLServer.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Sum { get; set; }
        public int SoldDateId { get; set; }
        public virtual SoldDate SoldDate { get; set; }
        public int SupermarketId { get; set; }
        public virtual Supermarket Supermarket { get; set; }
    }
}
