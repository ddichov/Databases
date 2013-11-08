using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarket.SQLServer.Models
{
    public class SoldDate
    {
        private ICollection<Sale> sales;

        public SoldDate()
        {
            this.sales = new HashSet<Sale>();
        }

        public int SoldDateId { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Sale> Sales
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
