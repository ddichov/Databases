using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.MongoDB.Client;
using SuperMarket.SQLServer.Data;
using MongoDB.Bson;
using System.Xml.Linq;
using SuperMarket.Formats;
using SuperMarket.SQLServer.Models;

namespace SuperMarket
{
    public class XmlToDatabasesExporter
    {
        public void Export(string xmlFullFileName, SupermarketContext sqlServerContext)
        {
            XDocument xmlDoc = XDocument.Load(xmlFullFileName);
            var sales = xmlDoc.Descendants("sale").ToList();

            int vendorId = -1;
            string vendorName = string.Empty;

            foreach (var sale in sales)
            {
                vendorName = sale.Attribute("vendor").Value;
                vendorId = sqlServerContext.Vendors.Where(x => x.VendorName == vendorName).Select(x => x.Id).FirstOrDefault();
                var expenses = sale.Descendants("expenses");

                foreach (var expense in expenses)
                {
                    MongoDbExpenseFormat expenseObject = new MongoDbExpenseFormat()
                    {
                        VendorName = vendorName,
                        VendorId = vendorId,
                        Month = DateTime.Parse(expense.Attribute("month").Value),
                        Expense = decimal.Parse(expense.Value),
                    };

                    MongoDbProvider.db.SaveData<MongoDbExpenseFormat>(expenseObject);

                    Expense sqlExpense = new Expense()
                    {
                        VendorId = expenseObject.VendorId,
                        Month = expenseObject.Month,
                        ExpenseValue = expenseObject.Expense
                    };

                    sqlServerContext.Expenses.Add(sqlExpense);
                    sqlServerContext.SaveChanges();
                }
            }
        }
    }
}
