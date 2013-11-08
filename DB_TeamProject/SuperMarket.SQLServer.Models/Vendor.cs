using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.SQLServer.Models
{
    public class Vendor
    {
        private ICollection<Product> products;
        private ICollection<Expense> expenses;

        public Vendor()
        {
            this.products = new HashSet<Product>();
            this.expenses = new HashSet<Expense>();
        }

        public int Id { get; set; }
        public string VendorName { get; set; }

        public virtual ICollection<Product> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }

        public virtual ICollection<Expense> Expenses
        {
            get
            {
                return this.expenses;
            }
            set
            {
                this.expenses= value;
            }
        }
    }
}
