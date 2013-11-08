using System;
using System.Linq;
using System.Text;
using SuperMarket.MySQL.Data;
using SuperMarket.SQLServer.Client;

namespace SuperMarket.MySQL.Client
{
    public class MySQLcontroller
    {
        public string ReadData(SQLServerController sqlServer)
        {
            StringBuilder sb = new StringBuilder();
            using (var context = new SuperMarketEntitiesModel())
            {
                sb.AppendLine("No Fetch optimization:");
                var query = context.Products.Where(p => p.ProductName.Contains("Coca"));
                sb.AppendLine("Product\tProductName");
                foreach (var product in query)
                {
                    
                    sb.AppendFormat("{0}\t{1}", product.ProductName, product.Vendor);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
