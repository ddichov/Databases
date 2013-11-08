using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.SQLServer.Data;
using SuperMarket.Formats;

namespace SuperMarket
{
    public static class SqlServerToPdf
    {
        public static IList<SqlPdfFormat> GenerateSalesReport(
            SupermarketContext sQLServerContext, 
            DateTime startDate, 
            DateTime endDate)
        {
            IList<SqlPdfFormat> data;
          
               data= sQLServerContext.Sales.Include("SoldDate.Date").Include("Product.ProductName").Include("Supermarket.Name")
                    .Where(x => x.SoldDate.Date >= startDate && x.SoldDate.Date <= endDate).Select(x => new SqlPdfFormat
                    {
                        ProductName = x.Product.ProductName,
                        Quantity = x.Quantity,
                        Measure = x.Product.Measure.MeasureName,
                        SoldDate = x.SoldDate.Date,
                        UnitPrice = x.UnitPrice,
                        SupermarketName = x.Supermarket.Name,
                        Sum = x.Sum
                    }).ToList();
           
            return data;
        }
    }
}
