using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarket.SQLServer.Models
{
    public class Supermarket
    {
        private ICollection<Sale> sales;

        public Supermarket()
        {
            this.sales = new HashSet<Sale>();
        }

        public int SupermarketId { get; set; }
        public string Name { get; set; }
        public ICollection<Sale> Sales
        {
            get
            {
                return this.sales;
            }
            set
            {
                this.sales = value;
            }
        }
    }
}
