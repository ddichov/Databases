using System;
using System.Linq;
using Northwind.Data;

namespace Ex10GetIncomsPerSupplier
{
    // 10. Create a stored procedures in the Northwind database for finding the total incomes 
    // for given supplier name and period (start date, end date). 
    // Implement a C# method that calls the stored procedure and returns the retuned record set.
    class IncomsPerSupplier
    {
        static void Main(string[] args)
        {
            //            string createProcString =
            //            @"CREATE PROC dbo.usp_FindIncomePerSupplier 
            //                @Company_Name nvarchar(40), @Beginning_Date DateTime, @Ending_Date DateTime
            //                AS
            //                    SELECT s.CompanyName, Orders.ShippedDate, Orders.OrderID, 'Order Subtotals'.Subtotal
            //                    FROM [Order Details] od 
            //                        JOIN Products p 
            //                        	ON od.ProductID=p.ProductID
            //                        JOIN Suppliers s 
            //                        	ON s.SupplierID=p.SupplierID
            //                        INNER JOIN 
            //                    (Orders INNER JOIN 'Order Subtotals' ON Orders.OrderID = 'Order Subtotals'.OrderID) 
            //                    ON od.OrderID = Orders.OrderID
            //                WHERE  s.CompanyName = @Company_Name AND Orders.ShippedDate Between @Beginning_Date And @Ending_Date";
            
            GetIncomsPerSupplier("New Orleans Cajun Delights", new DateTime(1994, 1, 1), new DateTime(2000, 12, 31));
        }

        static void GetIncomsPerSupplier(string supplierName, DateTime startDate, DateTime endDate)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();

            var salesByRegion =
                               from sale in northwindEntities.usp_FindIncomePerSupplier(supplierName, startDate, endDate)
                               select new
                               {
                                   ShippedDate = sale.ShippedDate,
                                   Subtotal = sale.Subtotal,
                                   Company = sale.CompanyName
                               };

            decimal? sum = 0;
            foreach (var item in salesByRegion)
            {
                // Console.WriteLine(item.ToString());
                sum += item.Subtotal;
            }

            Console.WriteLine("Total sales for the period = {0:C2}", sum);
            Console.WriteLine();
        }
    }
}