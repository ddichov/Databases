using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket;
using SuperMarket.MySQL.Client;
using SuperMarket.SQLServer.Client;
using SuperMarket.SQLServer.Data;
using SuperMarket.SQLServer.Models;
using System.Threading;
using System.Globalization;

namespace SuperMarket.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            MainController mainCon = MainController.Instance;

            mainCon.ExportFromMySqlToSqlServer();

            string zipFileLocation = @"..\..\..\Sales-Reports\";
            string zipFileName = @"Sales-Reports";
            string fileExtension = ".zip";
            string fullZipFileName = zipFileLocation + zipFileName + fileExtension;
            string extractionFolderFullPath = zipFileLocation + zipFileName + "\\";

            mainCon.ExportFromExcelToSqlServer(fullZipFileName, extractionFolderFullPath);

            DateTime startDate = new DateTime(2013, 6, 20);
            DateTime endDate = new DateTime(2013, 8, 20);
            string pdfFullFileName = @"..\..\..\Aggregated-Sales-Reports\Sample-Aggregated-Sales-Report.pdf";
            string cssFullFileName = @"..\..\..\CSS\style.css";
            mainCon.CreatePdfReport(startDate, endDate, pdfFullFileName, cssFullFileName);

            //test xml
            string salesByVendorsXmlFullFileName = "Sales-by-Vendors-report";
            mainCon.GenerateSqlToXmlVendorsReport(startDate, endDate, salesByVendorsXmlFullFileName);
            

            string location = @"..\..\..\Product-Reports\";
            mainCon.ExportDataToMongoDb(location);

            string expensesXmlFullFileName = @"..\..\..\Expenses\Vendors-Expenses.xml";
            mainCon.ExportDataFromXmlToDatabase(expensesXmlFullFileName);

            mainCon.ExportFromMongoDBToSQLite();

            string excelFileFullName = @"..\..\..\Product-Reports\Products-Total-Report.xlsx";
            mainCon.ExportDataToExcelFile(excelFileFullName);
        }
    }
}
