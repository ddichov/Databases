using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex04AddsNewProduct
{
    //// Write a method that adds a new product in the products table in the Northwind database. 
    //// Use a parameterized SQL command.
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection("Server=.; " +
            "Database=Northwind; Integrated Security=true");
            dbCon.Open();

            int insertedRows = InsertProduct("fffff", 2, 3, "ttttt", 20.5M, 10, 20, 1, 1, dbCon);
            Console.WriteLine("insertedRows {0}", insertedRows);
        }

        private static int InsertProduct(string name, int? supplierID, int? categoryID, string quantityPerUnit, 
            decimal? unitPrice, int? unitsInStock, int? unitsOnOrder, int? reorderLevel, byte discontinued,
            SqlConnection dbCon)
        {
            SqlCommand cmdInsertProduct = new SqlCommand(
                  "INSERT INTO Products VALUES " +
                  "(@name, @supplierID, @categoryID, @quantityPerUnit, @unitPrice, @unitsInStock, @unitsOnOrder, @reorderLevel,  @discontinued)", dbCon);
            cmdInsertProduct.Parameters.AddWithValue("@name", name);
            cmdInsertProduct.Parameters.AddWithValue("@supplierID", supplierID);
            cmdInsertProduct.Parameters.AddWithValue("@categoryID", categoryID);
            cmdInsertProduct.Parameters.AddWithValue("@quantityPerUnit", quantityPerUnit);
            cmdInsertProduct.Parameters.AddWithValue("@unitPrice", unitPrice);
            cmdInsertProduct.Parameters.AddWithValue("@unitsInStock", unitsInStock);
            cmdInsertProduct.Parameters.AddWithValue("@unitsOnOrder", unitsOnOrder);
            cmdInsertProduct.Parameters.AddWithValue("@reorderLevel", reorderLevel);
            cmdInsertProduct.Parameters.AddWithValue("@discontinued", discontinued);

            cmdInsertProduct.ExecuteNonQuery();

            SqlCommand cmdSelectIdentity =
            new SqlCommand("SELECT @@Identity", dbCon);
            int insertedRecordId = (int)(decimal)cmdSelectIdentity.ExecuteScalar();
            return insertedRecordId;
        }
    }
}
