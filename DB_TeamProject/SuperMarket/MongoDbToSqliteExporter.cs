using SuperMarket.Formats;
using SuperMarket.MongoDB.Client;
using SuperMarket.SQLite.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket
{
    public class MongoDbToSqliteExporter
    {
        public void Export(SuperMarketEntities sqliteContext)
        {
            var query = MongoDbProvider.db.LoadData<MongoDbProductFormat>().AsQueryable();
            foreach (var item in query)
            {
                sqliteContext.Products.Add(new Product()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    VendorName = item.VendorName,
                    TotalIncomes = item.TotalIncomes
                });
            }
            sqliteContext.SaveChanges();
        }
    }
}
