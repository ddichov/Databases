using System;
using System.Linq;
using Northwind.Data;

namespace Ex09OrderCreator
{
    // 09. Create a method that places a new order in the Northwind database. 
    // The order should contain several order items. Use transaction to ensure the data consistency.
    class OrderCreator
    {
        static void Main(string[] args)
        {
            Order newOrder = new Order();
            newOrder.CustomerID = "VINET";
            newOrder.EmployeeID = 5;
            newOrder.OrderDate = new DateTime(2013, 5, 1);
            newOrder.RequiredDate = new DateTime(2013, 8, 2);
            newOrder.ShippedDate = new DateTime(2013, 8, 1);
            newOrder.ShipVia = 3;
            newOrder.Freight = 20.05M;
            newOrder.ShipName = "Altaro rego";

            int result = AddExistingCustomerOrder(newOrder);
            Console.WriteLine(result);
        }

        public static int AddExistingCustomerOrder(Order newOrder)
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                var customer = db.Customers.First(c => c.CustomerID == newOrder.CustomerID);
                if (customer != null)
                {
                    newOrder.ShipAddress = customer.Address;
                    newOrder.ShipCity = customer.City;
                    newOrder.ShipRegion = customer.Region;
                    newOrder.ShipPostalCode = customer.PostalCode;
                    newOrder.ShipCountry = customer.Country;

                    db.Orders.Add(newOrder);
                    db.Orders.Add(newOrder);
                    db.Orders.Add(newOrder);
                    
                    return db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("No such Customer in Database.", "newOrder");
                }
            }
        }
    }
}