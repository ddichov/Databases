using System;
using System.Data.SqlClient;
using System.Linq;

namespace EX03GetProductCategories
{
    // Write a program that retrieves from the Northwind database 
    // all product categories and the names of the products in each category. 
    // Can you do this with a single SQL query (with table join)?
    class GetProductCategories
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection("Server=.; " +
            "Database=Northwind; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                Console.WriteLine("All Categories with Products:");
                SqlCommand cmdAllCategories = new SqlCommand(
                  "SELECT c.CategoryName, p.ProductName FROM Products p LEFT JOIN Categories c ON p.CategoryID=c.CategoryID", dbCon);
                SqlDataReader reader = cmdAllCategories.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string categoryName = (string)reader["CategoryName"];
                        string product = (string)reader["ProductName"];

                        Console.WriteLine("{0}: {1}", categoryName, product);
                    }
                }
            }
        }
    }
}
