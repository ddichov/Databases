using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.Formats;
using SuperMarket.MongoDB.Client;
using SuperMarket.SQLServer.Data;
using SuperMarket.SQLServer.Models;
using System.IO;
using System.Web.Script.Serialization;

namespace SuperMarket
{
    public class SqlServerDataToMongoDbExporter
    {
        public void Export(SupermarketContext sQLServerContext, string location)
        {
            foreach (var product in sQLServerContext.Products.Include("Vendor"))
            {
                using (SupermarketContext sqlServerInnerContext = new SupermarketContext())
                {
                    int quantitySold = sqlServerInnerContext.Sales.Include("Product.ProductName").Where(x => x.Product.ProductName == product.ProductName).Select(x => x.Quantity).Sum();
                    decimal totalIncome = sqlServerInnerContext.Sales.Include("Product.ProductName").Where(x => x.Product.ProductName == product.ProductName).Select(x => x.Sum).Sum();

                    MongoDbProductFormat newExportObject = new MongoDbProductFormat()
                    {
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        VendorName = product.Vendor.VendorName,
                        TotalQuantitySold = quantitySold,
                        TotalIncomes = totalIncome
                    };

                    MongoDbProvider.db.SaveData<MongoDbProductFormat>(newExportObject);
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}", product.Id, product.ProductName, product.Vendor.VendorName, quantitySold, totalIncome);

                    ExportToJSON(newExportObject, location);
                }
            }
        }

        private void ExportToJSON(MongoDbProductFormat exportObject, string location)
        {
            DirectoryInfo dir = new DirectoryInfo(location);
            string fileFullPath = dir.FullName + exportObject.ProductId + ".json";
            FileStream file = File.Create(fileFullPath);
            file.Close();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(exportObject);
            File.WriteAllText(fileFullPath, json);
        }
    }
}
