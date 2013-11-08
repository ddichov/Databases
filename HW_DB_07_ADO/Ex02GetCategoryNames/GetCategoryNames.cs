using System;
using System.Data.SqlClient;
using System.Linq;

namespace Ex02GetCategoryNames
{
    //// Write a program that retrieves the name and description of all categories in the Northwind DB.
    class GetCategoryNames
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection("Server=.; " +
            "Database=Northwind; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                Console.WriteLine("All Categories with Description:");
                SqlCommand cmdAllCategories = new SqlCommand(
                  "SELECT CategoryName, Description FROM Categories", dbCon);
                SqlDataReader reader = cmdAllCategories.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string categoryName = (string)reader["CategoryName"];
                        string description = (string)reader["Description"];

                        Console.WriteLine("{0}: {1}", categoryName, description);
                    }
                }
            }
        }
    }
}
