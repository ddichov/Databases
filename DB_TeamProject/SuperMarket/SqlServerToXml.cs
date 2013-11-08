using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.SQLServer.Data;
using SuperMarket.Formats;
using System.Xml;
using SuperMarket.Formats;
using System.Globalization;

namespace SuperMarket
{
    class SqlServerToXml
    {
        public static IList<SqlXmlFormat> GenerateXmlSalesReport(
            SupermarketContext sQLServerContext,
            DateTime startDate,
            DateTime endDate)
        {
            IList<SqlXmlFormat> data;
            /*
            string sqlString = @"SELECT v.VendorName, sd.Date, SUM(s.Sum) FROM Vendors v
            LEFT JOIN Products p ON p.VendorId =  v.Id
            LEFT JOIN Sales s ON s.ProductId = p.Id
            LEFT JOIN SoldDates sd ON s.SoldDateId = sd.SoldDateId
            GROUP BY sd.Date, v.VendorName";
            */
            var result =
                        from v in sQLServerContext.Vendors
                        join p in sQLServerContext.Products on v.Id equals p.VendorId
                        join s in sQLServerContext.Sales on p.Id equals s.ProductId
                        join sd in sQLServerContext.Sales on s.SoldDateId equals sd.SoldDateId
                        where sd.SoldDate.Date >= startDate && sd.SoldDate.Date <= endDate
                        group v by new
                        {
                            v.VendorName,
                            sd.SoldDate,
                            s.Sum
                        }
                        into x orderby x.Key.VendorName
                               select new SqlXmlFormat
                               {
                                   VendorName = x.Key.VendorName,
                                   SoldDate = x.Key.SoldDate.Date,
                                   Sum = x.Key.Sum
                               };
          
            data = result.ToList();
            return data;
        }

        public static void WriteXmlSalesReport(IList<SqlXmlFormat> data, string fileName)
        {
            fileName = "../../../Sales-By-Vendors/" + fileName + ".xml";
            Encoding encoding = Encoding.GetEncoding("windows-1251");

            data.OrderBy(x => x.VendorName);
           
            using (XmlTextWriter writer = new XmlTextWriter(fileName, encoding))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = '\t';
                writer.Indentation = 1;

                writer.WriteStartDocument();
                writer.WriteStartElement("sales");

                string currentVendorName = data[0].VendorName;
                writer.WriteStartElement("sale");
                writer.WriteAttributeString("vendor", currentVendorName);

                for (int i = 0; i < data.Count; i++)
                {
                    if (currentVendorName != data[i].VendorName)
                    {
                        writer.WriteEndElement();
                        currentVendorName = data[i].VendorName;

                        writer.WriteStartElement("sale");
                        writer.WriteAttributeString("vendor", currentVendorName);
                    }
                    
                    writer.WriteStartElement("summary");
                    writer.WriteAttributeString("date", data[i].SoldDate.ToString("dd-MMM-yyyy", DateTimeFormatInfo.InvariantInfo));
                    writer.WriteAttributeString("total-sum", data[i].Sum.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}