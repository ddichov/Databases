using System;
using System.Linq;
using Northwind.Data;

namespace Ex07ConcurrentChanges
{
    //// 07 Try to open two different data contexts and perform concurrent changes 
    //// on the same records. What will happen at SaveChanges()? 
    class ConcurrentChanges
    {
        //// This will produce an error!
        static void Main(string[] args)
        {
            Customer newCustmer = new Customer();
            newCustmer.CustomerID = "KULO";
            newCustmer.CompanyName = "Mala";
            newCustmer.ContactName = "Misoto Kulano";
            newCustmer.ContactTitle = "Owner";
            newCustmer.Address = "Amela str 23";
            newCustmer.City = "Pelon";
            newCustmer.PostalCode = "1231";
            newCustmer.Country = "France";
            newCustmer.Phone = "3443-4323-432";
            newCustmer.Fax = "3245-243";
            using (NorthwindEntities northwindDB = new NorthwindEntities())
            {
                using (NorthwindEntities secondDB = new NorthwindEntities())
                {
                    northwindDB.Customers.Add(newCustmer);
                    northwindDB.SaveChanges();

                    // Customer customer = secondDB.Customers.FirstOrDefault(x => x.CustomerID == "KULO");
                    // Console.WriteLine(customer);

                    secondDB.Customers.Attach(newCustmer);
                    secondDB.Customers.Remove(newCustmer);
                    secondDB.SaveChanges();
                }
            }
        }
    }
}