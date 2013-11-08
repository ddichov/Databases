using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex01GetNumberOfRows
{
    //// Write a program that retrieves from the Northwind sample database 
    //// in MS SQL Server the number of  rows in the Categories table.
    class GetNumberOfRows
    {
        static void Main()
        {
            SqlConnection dbCon = new SqlConnection("Server=.; " +
            "Database=Northwind; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                SqlCommand cmdCount = new SqlCommand(
                    "SELECT COUNT(*) FROM Categories", dbCon);
                int categoriesCount = (int)cmdCount.ExecuteScalar();
                Console.WriteLine("Categories count: {0} ", categoriesCount);
                Console.WriteLine();
            }
        }
    }
}
