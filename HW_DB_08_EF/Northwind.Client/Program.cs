using System;
using System.Linq;
using Northwind.Data;

namespace Northwind.Client
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Program started.");
            PrintLastFiveCustomers();

            string newCustomerId = CreateNewCustomer("a0001", "Company A", "Ivan", "Mr", "address1",
                "city 1", "region1", "code", "country",
                "phone", "fax");
            Console.WriteLine("Created new Customer.");
            PrintLastFiveCustomers();

            ModifyCustomerContactNameAndTitle(newCustomerId, "new name ", "new Boss");
            Console.WriteLine("Modified the Customer {0}.", newCustomerId);
            PrintLastFiveCustomers();

            Console.WriteLine("Deleted the Customer {0}.", newCustomerId);
            DeleteCustomer(newCustomerId);
            PrintLastFiveCustomers();

            //// Ex 03
            Console.WriteLine("EX 03. All customers who have orders made in 1997 and shipped to Canada.");
            PrintCustomersWithOrdersToCanada();

            //// Ex 04
            Console.WriteLine("EX 04 SQL. All customers who have orders made in 1997 and shipped to Canada.");
            PrintCustomersWithOrdersToCanadaNativeSql(1997, "Canada");

            //// Ex 05
            Console.WriteLine("EX 05. All sales by specified region and period.");
            GetSalesByRegion("WA", new DateTime(1997, 10, 01), new DateTime(1997, 12, 31));
        }

        static void PrintLastFiveCustomers()
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            var lastFiveCustomers =
                (from c in northwindEntities.Customers
                 orderby c.CustomerID descending
                 select c).Take(5);

            Console.WriteLine("Last 5 customers:");
            foreach (var customer in lastFiveCustomers)
            {
                Console.WriteLine("{0}. {1}, Contact: {2} {3}",
                    customer.CustomerID, customer.CompanyName, customer.ContactTitle, customer.ContactName);
            }

            Console.WriteLine();
        }

        static string CreateNewCustomer(
            string customerID,
            string companyName,
            string contactName,
            string contactTitle,
            string address,
            string city,
            string region,
            string postalCode,
            string country,
            string phone,
            string fax)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            Customer newCustomer = new Customer
            {
                CustomerID = customerID,
                CompanyName = companyName,
                ContactName = contactName,
                ContactTitle = contactTitle,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country,
                Phone = phone,
                Fax = fax
            };
            northwindEntities.Customers.Add(newCustomer);
            northwindEntities.SaveChanges();
            return newCustomer.CustomerID;
        }

        static void ModifyCustomerContactNameAndTitle(string customerID, string newName, string newTitle)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            Customer customer = GetCustomerById(northwindEntities, customerID);
            customer.ContactName = newName;
            customer.ContactTitle = newTitle;
            northwindEntities.SaveChanges();
        }

        static void DeleteCustomer(string customerID)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            Customer customer = GetCustomerById(northwindEntities, customerID);
            northwindEntities.Customers.Remove(customer);
            northwindEntities.SaveChanges();
        }

        static Customer GetCustomerById(NorthwindEntities northwindEntities, string customerID)
        {
            Customer customer = northwindEntities.Customers.FirstOrDefault(
                c => c.CustomerID == customerID);
            return customer;
        }

        // Ex 03
        static void PrintCustomersWithOrdersToCanada()
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            var results =
                         from order in northwindEntities.Orders
                         where order.OrderDate.Value.Year == 1997 && order.ShipCountry == "Canada"
                         select new
                         {
                             CustomerName = order.Customer.CompanyName,
                             OrderDate = order.OrderDate,
                             Country = order.ShipCountry
                         };

            foreach (var item in results)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine();
        }

        // Ex 04
        static void PrintCustomersWithOrdersToCanadaNativeSql (int year, string country)
        {
            using (NorthwindEntities northwindEntities = new NorthwindEntities())
            {
                var results = northwindEntities.Database.SqlQuery<Customer>(
                                                                            "Select DISTINCT c.* " +
                                                                            "From Customers c Join Orders o ON c.CustomerID = o.CustomerID " +
                                                                            "WHERE {0}= Year(o.OrderDate) AND o.ShipCountry={1}", year, country);

                Console.WriteLine(string.Join(Environment.NewLine, results));
            }
            Console.WriteLine();
        }

        // Ex 05
        static void GetSalesByRegion(string region, DateTime startDate, DateTime endDate)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();

            var salesByRegion =
                               from sale in northwindEntities.Sales_by_Year(startDate, endDate)
                               join order in northwindEntities.Orders on sale.OrderID equals order.OrderID
                               join employee in northwindEntities.Employees on order.EmployeeID equals employee.EmployeeID
                               where employee.Region == region
                               select new
                               {
                                   OrderDate = order.OrderDate,
                                   Subtotal = sale.Subtotal,
                                   OrderRegion = employee.Region
                               };

            decimal? sum = 0;
            foreach (var item in salesByRegion)
            {
                Console.WriteLine(item.ToString());
                sum += item.Subtotal;
            }

            Console.WriteLine("Total sales for the period = {0}", sum);
            Console.WriteLine();
        }
        
    }
}