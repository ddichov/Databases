using System;
using System.Data.OleDb;
using System.Linq;

namespace Ex06_07XlsNameScore
{
    //// Create an Excel file with 2 columns: name and score.
    //// Write a program that reads your MS Excel file through the 
    //// OLE DB data provider and displays the name and score row by row.
    //// Implement appending new rows to the Excel file.
    class XlsNameScore
    {
        static void Main(string[] args)
        {
            OleDbConnection db = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;" +
            @"Data Source=..\scores.xls;Extended Properties=""Excel 8.0;HDR=Yes;ReadOnly=False;""");
            PrintAllData(db);

            OleDbCommand addRow = new OleDbCommand("INSERT INTO [Sheet1$] VALUES ('Nickey Kolev', 29)", db);
            Console.WriteLine();

            // Add a row
            db.Open();

            try
            {
                addRow.ExecuteNonQuery();
                Console.WriteLine("Nickey Kolev, 29 Added.");
                Console.WriteLine();
            }
            catch (OleDbException oleEx)
            {
                Console.WriteLine("OLEDB Error: {0}", oleEx.Message);
            }
            catch (InvalidOperationException invalex)
            {
                Console.WriteLine("Operation Error: {0}", invalex.Message);
                Console.WriteLine();
            }
            db.Close();

            // Reading data
            try
            {
                PrintAllData(db);
            }
            catch (InvalidOperationException invalex)
            {
                Console.WriteLine("Operation Error: {0}", invalex.Message);
                Console.WriteLine();
            }
        }

        private static void PrintAllData(OleDbConnection db)
        {
            OleDbCommand selectRows = new OleDbCommand("select * from [Sheet1$]", db);
            db.Open();

            OleDbDataReader reader = selectRows.ExecuteReader();

            Console.WriteLine("All items in \\bin\\scores.xls:");
            while (reader.Read())
            {
                string name = (string)reader["Name"];
                double score = (double)reader["Score"];
                Console.WriteLine("{0} - score: {1}", name, score);
            }

            db.Close();
        }
    }
}
