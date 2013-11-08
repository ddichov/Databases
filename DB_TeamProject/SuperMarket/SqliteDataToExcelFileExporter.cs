using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.SQLite.DataProvider;
using System.Data.OleDb;
using OfficeOpenXml.Utils;
using OfficeOpenXml;
using System.IO;
using SuperMarket.Formats;
using SuperMarket.SQLServer.Data;

namespace SuperMarket
{
    public class SqliteDataToExcelFileExporter
    {
        private void CreateExcelFile(string fileName, IList<SqliteExcelFormat> data)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {
                var ws = excel.Workbook.Worksheets.Add("Sheet1");
                ws.Cells[1, 1].Value = "Vendor";
                ws.Cells[1, 2].Value = "Incomes";
                ws.Cells[1, 3].Value = "Expenses";
                ws.Cells[1, 4].Value = "Taxes";
                ws.Cells[1, 5].Value = "Financial Result";

                for (int i = 0; i < data.Count; i++)
                {
                    var tax = (data[i].TaxPercentage / 100) * data[i].Incomes;
                    ws.Cells[i + 2, 1].Value = data[i].Vendor;
                    ws.Cells[i + 2, 2].Value = data[i].Incomes;
                    ws.Cells[i + 2, 3].Value = data[i].Expenses;
                    ws.Cells[i + 2, 4].Value = Math.Round((decimal)tax, 2);
                    ws.Cells[i + 2, 5].Value = Math.Round((decimal)(data[i].Incomes - data[i].Expenses - tax), 2);
                }
                            
                ws.Column(1).AutoFit();
                ws.Column(2).AutoFit();
                ws.Column(3).AutoFit();
                ws.Column(4).AutoFit();
                ws.Column(5).AutoFit();
                excel.Save();
            }
        }

        public IList<SqliteExcelFormat> GetDataForExcelFile(SuperMarketEntities sqliteContext, SupermarketContext sqlServerContext)
        {
            IList<SqliteExcelFormat> data;

            data = sqliteContext.Products.Include("TaxTables.Tax").Select(x => new SqliteExcelFormat
            {
                Vendor = x.VendorName,
                Incomes = x.TotalIncomes,
                TaxPercentage = x.TaxTable.Tax,
                ProductId = x.ProductId
            }).ToList();

            foreach (var item in data)
            {
                //item.Expenses = sqlServerContext.Expenses.Where(x => x.Vendor.VendorName == item.Vendor).Select(x => x.ExpenseValue).First();
                var vendorId = sqlServerContext.Products.Where(x => item.ProductId == x.Id).Select(x => x.VendorId).First();
                item.Expenses = sqlServerContext.Expenses.Where(x => x.VendorId == vendorId).Select(x => x.ExpenseValue).First();
            }

            return data;
        }


        public void Export(SuperMarketEntities sqliteContext, SupermarketContext sqlServerContext, string fileName)
        {
            IList<SqliteExcelFormat> data = GetDataForExcelFile(sqliteContext, sqlServerContext);
            CreateExcelFile(fileName, data);
        }
    }
}
