using System;
using System.Data.SqlClient;
using System.Linq;

namespace Ex08FindProductsByString
{
    class FindProductsByString
    {
        //// Write a program that reads a string from the console and finds all products that contain this string. 
        //// Ensure you handle correctly characters like ', %, ", \ and _.
        static void Main(string[] args)
        {
            Console.WriteLine("Find a Product by a search term.\n");
            Console.Write("Enter a search term: ");

            string searchTerm = Console.ReadLine();

            SqlConnection db = new SqlConnection("Server=.; Database=NORTHWND; Integrated Security=true");

            // Uncomment if express:
            // SqlConnection db = new SqlConnection("Server=SQLEXPRESS; Database=NORTHWND; Integrated Security=true");

            db.Open();
            using (db)
            {
                SqlCommand searchQuery = new SqlCommand("SELECT ProductName FROM Products WHERE ProductName LIKE @searchTerm", db);
                SqlParameter searchTermParam = new SqlParameter("@searchTerm", '%' + searchTerm + '%');
                searchQuery.Parameters.Add(searchTermParam);

                SqlDataReader reader = searchQuery.ExecuteReader();

                using (reader)
                {
                    Console.WriteLine("\nFound Products:");

                    while (reader.Read())
                    {
                        string product = (string)reader["ProductName"];
                        Console.WriteLine(product);
                    }
                }
            }
        }
    }
}
