using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.Formats;
using SuperMarket.MongoDB.Client;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System.IO;

namespace SuperMarket.PDF.Client
{
    public static class PdfGenerator
    {
        public static void GeneratePdf(IList<SqlPdfFormat> data, string pdfFullFileName, string cssFullFileName)
        {
            data.OrderBy(x => x.SoldDate);
            DateTime oldDate = data[0].SoldDate;
            StringBuilder sb = new StringBuilder();
            decimal subSum = 0;
            decimal totalSum = 0;
           
            sb.Append("<table>");
            sb.Append("<tr>");
            sb.Append("<th colspan=\"5\" class=\"title\">Aggregated Sales Report </th>");
            sb.Append("</tr>");

            sb.Append("<tr class=\"grayBack\" >");
            sb.AppendFormat("<th colspan=\"5\" class=\"date\">Date: {0}</th>", oldDate.ToShortDateString());
            sb.Append("</tr>");
            sb.Append("<tr class=\"grayBack\" >");
            sb.Append("<th  class=\"th\">Product</th>");
            sb.Append("<th  class=\"th\">Quantity</th>");
            sb.Append("<th  class=\"th\">Unit Price</th>");
            sb.Append("<th  class=\"th\">Location</th>");
            sb.Append("<th  class=\"th\">Sum</th>");
            sb.Append("</tr>");

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].SoldDate > oldDate)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td colspan=\"4\" class=\"sum\">Total sum for {0}:&nbsp;</td>", oldDate.ToShortDateString());
                    sb.AppendFormat("<td class=\"bold\">{0}</td>", subSum);
                    sb.Append("</tr>");

                    oldDate = data[i].SoldDate;
                    totalSum += subSum;
                    subSum = 0;

                    sb.Append("<tr class=\"grayBack\">");
                    sb.AppendFormat("<th colspan=\"5\" class=\"date\">Date: {0}</th>", oldDate.ToShortDateString());
                    sb.Append("</tr>");
                    sb.Append("<tr class=\"grayBack\">");
                    sb.Append("<th  class=\"th\">Product</th>");
                    sb.Append("<th  class=\"th\">Quantity</th>");
                    sb.Append("<th  class=\"th\">Unit Price</th>");
                    sb.Append("<th  class=\"th\">Location</th>");
                    sb.Append("<th  class=\"th\">Sum</th>");
                    sb.Append("</tr>");
                }

                subSum += data[i].Sum;

                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", data[i].ProductName);
                sb.AppendFormat("<td>{0}</td>", data[i].Quantity + " " + data[i].Measure);
                sb.AppendFormat("<td>{0}</td>", data[i].UnitPrice);
                sb.AppendFormat("<td>{0}</td>", data[i].SupermarketName);
                sb.AppendFormat("<td>{0}</td>", data[i].Sum);
                sb.Append("</tr>");
            }

            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"4\" class=\"sum\">Total sum for {0}:&nbsp;</td>", oldDate.ToShortDateString());
            sb.AppendFormat("<td class=\"bold\">{0}</td>", subSum);
            sb.Append("</tr>");

            totalSum += subSum;

            sb.Append("<tr>");
            sb.Append("<td colspan=\"4\" class=\"sum\">Grand Total:&nbsp;</td>");
            sb.AppendFormat("<td class=\"bold\">{0}</td>", totalSum);
            sb.Append("</tr>");
            sb.Append("</table>");
            
            using (Document document = new Document())
            {
                PdfWriter writer = PdfWriter.GetInstance(document,
                    new FileStream(pdfFullFileName, FileMode.Create));
                document.Open();

                HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                ICSSResolver cssResolver =
                    XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
                //change this to your CCS file location
                cssResolver.AddCssFile(cssFullFileName, true);
                IPipeline pipeline =
                    new CssResolverPipeline(cssResolver,
                        new HtmlPipeline(htmlContext,
                            new PdfWriterPipeline(document, writer)));

                XMLWorker worker = new XMLWorker(pipeline, true);
                XMLParser p = new XMLParser(worker);
                //p.Parse((TextReader)File.OpenText(@"G:\Template.html"));
                p.Parse(new StringReader(sb.ToString()));
            }
        }
    }
}