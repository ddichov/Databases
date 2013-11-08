namespace SuperMarket
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using SuperMarket.MySQL.Data;
    using SuperMarket.SQLServer.Client;
    using SuperMarket.Excel.Client;
    using SuperMarket.SQLServer.Data;
    using System.Data.OleDb;
    using SuperMarket.SQLServer.Data.Migrations;
    using SuperMarket.SQLServer.Models;
    using System.Collections.Generic;
    using SuperMarket.PDF.Client;
    using SuperMarket.SQLite.Data;
    using SuperMarket.Formats;
    using SuperMarket.MongoDB.Client;
    using System.Text;
    using SuperMarket.SQLite.DataProvider;

    public class MainController
    {
        private static MainController instance;
        
        private SupermarketContext sQLServerContext;
        private SuperMarketEntitiesModel mySqlContext;
        private SupermarketSQLiteContext sQLiteServerContext;
        private OleDbConnection excelConnection;
        private SuperMarketEntities sqliteContext;


        private MainController()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<SupermarketContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<SupermarketContext, Configuration>());
            this.sQLServerContext = new SupermarketContext();
            this.mySqlContext = new SuperMarketEntitiesModel();
            this.excelConnection = new OleDbConnection();
            this.sqliteContext = new SuperMarketEntities();
        }

        public static MainController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainController();
                }
                return instance;
            }            
        }

        public void ExportFromMySqlToSqlServer()
        {
            FromMySQLToSQLServer mySqlToSqlServer = 
                new FromMySQLToSQLServer(sQLServerContext, mySqlContext);
            mySqlToSqlServer.Export();
        }

        public void ExportFromExcelToSqlServer(string zipFileName, string extractionFolder)
        {
            ExcelToSqlServerExporter exporter = 
                new ExcelToSqlServerExporter();
            exporter.Export(zipFileName, extractionFolder, excelConnection, sQLServerContext);
        }

        public void FreeResourses()
        {
        }

        public void CreatePdfReport(DateTime startDate, DateTime endDate, string pdfFullFileName, string cssFullFileName)
        {
            IList<SqlPdfFormat> data = 
                SqlServerToPdf.GenerateSalesReport(sQLServerContext, startDate, endDate);
            PdfGenerator.GeneratePdf(data, pdfFullFileName, cssFullFileName);
        }

        public void GenerateSqlToXmlVendorsReport(DateTime startDate, DateTime endDate, string fileName)
        {
           var data= SqlServerToXml.GenerateXmlSalesReport(sQLServerContext, startDate, endDate);
           SqlServerToXml.WriteXmlSalesReport(data, fileName);
        }

        public void ExportFromMongoDBToSQLite()
        {
            //test
            //this.sqliteContext.Products.Add(new SuperMarket.SQLite.DataProvider.Product()
            //{
            //    ProductId = 1,
            //    ProductName = "Cakes",
            //    VendorName = "Milka",
            //    TotalIncomes = 200.07M
            //});

            //Console.WriteLine(this.sqliteContext.Database.Connection.ConnectionString);
            //this.sqliteContext.SaveChanges();

            //var test = sQLiteServerContext.Taxes.All(t => t.ProductName == "test");
            //Console.WriteLine(test.ToString());

            MongoDbToSqliteExporter exporter = new MongoDbToSqliteExporter();
            exporter.Export(this.sqliteContext);
        }

        public void ExportDataToMongoDb(string location)
        {
            SqlServerDataToMongoDbExporter exporter = new SqlServerDataToMongoDbExporter();
            exporter.Export(this.sQLServerContext, location);
        }

        public void ExportDataFromXmlToDatabase(string xmlFullFileName)
        {
            XmlToDatabasesExporter exporter = new XmlToDatabasesExporter();
            exporter.Export(xmlFullFileName, this.sQLServerContext);
        }

        public void ExportDataToExcelFile(string fileName)
        {
            SqliteDataToExcelFileExporter exporter = new SqliteDataToExcelFileExporter();
            exporter.Export(this.sqliteContext, this.sQLServerContext, fileName);
        }
    }
}
