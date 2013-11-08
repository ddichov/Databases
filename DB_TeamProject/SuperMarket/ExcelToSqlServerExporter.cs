using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using System.IO;
using System.Data.OleDb;
using SuperMarket.SQLServer.Data;
using SuperMarket.SQLServer.Models;
using System.Data.Entity;
using System.Data;

namespace SuperMarket
{
    public class ExcelToSqlServerExporter
    {
        private static ICollection<ZipEntry> GetEntries(string zipFileName)
        {
            ICollection<ZipEntry> files;
            using (ZipFile zipFile = ZipFile.Read(zipFileName))
            {
                files = zipFile.SelectEntries("name = *.xls");
            }
            return files;
        }

        private static void ExtractTo(string zipFileName, string extractionFolder)
        {
            ICollection<ZipEntry> files = GetEntries(zipFileName);
            foreach (ZipEntry file in files)
            {
                file.Extract(extractionFolder, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        private void ExportSingleExcelFile(FileInfo file, int dateId, 
            OleDbConnection excelConnection, SupermarketContext sqlServerDb)
        {
            excelConnection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + 
                string.Format("Data Source={0};", file.FullName) +
                @"Extended Properties=""Excel 12.0;HDR=YES""";
            excelConnection.Open();

            int supermarketId = -1;
            string supermarketName = file.Name.Substring(0, file.Name.IndexOf("-Sales-"));

            if (sqlServerDb.Supermarkets.
                       Where(x => x.Name == supermarketName).Count() == 0)
            {
                sqlServerDb.Supermarkets.Add(new Supermarket()
                {
                    Name = supermarketName
                });
                sqlServerDb.SaveChanges();
            }
            supermarketId = sqlServerDb.Supermarkets.
                Where(x => x.Name == supermarketName).
                Select(x => x.SupermarketId).First();

            DataTable table = new DataTable();

            using (excelConnection)
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(@"SELECT * FROM [Sales$]", excelConnection);
                
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count - 1; i++)
                {
                    int productId = -1, quantity = -1;
                    decimal unitPrice = -1, sum = -1;

                    IEnumerable<object> data = table.Rows[i].ItemArray.SkipWhile(x => x.GetType().Equals(typeof(System.DBNull)));
                    int currentColumnIndex = 0;
                    foreach (var item in data)
                    {
                        if (item.GetType() == typeof(System.DBNull))
                        {
                            //Console.WriteLine(item.GetType());
                            continue;
                        }
                        else
                        {
                            switch (currentColumnIndex)
                            {
                                case 0:
                                    {
                                        productId = (int)((double) item);
                                        break;
                                    }
                                case 1:
                                    {
                                        quantity = (int)((double) item);
                                        break;
                                    }
                                case 2:
                                    {
                                        unitPrice = (decimal)((double) item);
                                        break;
                                    }
                                case 3:
                                    {
                                        sum = (decimal)((double) item);
                                        break;
                                    }

                            }   
                        }

                        currentColumnIndex++;
                    }

                    if (productId != -1)
                    {
                        //Console.WriteLine("{0}. {1} -> {2}", i, currentColumnIndex, item);
                        AddSale(productId, quantity, unitPrice, sum,
                            sqlServerDb, dateId, supermarketId);
                    }
                }
            }
        }

        private void AddSale(int productId, int quantity, decimal unitPrice, decimal sum, 
            SupermarketContext sqlServerDb, int dateId, int supermarketId)
        {
            Sale sale = new Sale()
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Sum = sum,
                SoldDateId = dateId,
                SupermarketId = supermarketId
            };

            sqlServerDb.Sales.Add(sale);
            sqlServerDb.SaveChanges();
        }

        public void Export(string zipFileName, string extractionFolder, 
                           OleDbConnection excelConnection, SupermarketContext sqlServerDb)
        {
            ExtractTo(zipFileName, extractionFolder);
            DirectoryInfo directory = new DirectoryInfo(extractionFolder);
            foreach (var dir in directory.GetDirectories())
            {
                int dateId = -1;
                //Console.WriteLine("{0}", dir.Name);
                DateTime date = DateTime.Parse(dir.Name);
                if (sqlServerDb.SoldDates.Where(x => x.Date == date).Count() == 0)
                {
                    sqlServerDb.SoldDates.Add(new SoldDate()
                    {
                        Date = date
                    });
                    sqlServerDb.SaveChanges();
                }
                dateId = sqlServerDb.SoldDates.Where(x => x.Date.Equals(date)).Select(x => x.SoldDateId).First();

                foreach (var file in dir.GetFiles())
                {
                    ExportSingleExcelFile(file, dateId, excelConnection, sqlServerDb);
                }
            }

        }
    }
}
