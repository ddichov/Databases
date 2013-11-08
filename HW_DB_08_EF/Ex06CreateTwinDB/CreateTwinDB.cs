using System;
using System.Linq;
using Northwind.Data;
using System.Data.Entity.Infrastructure;////ex 06
using System.Data.SqlClient; 

namespace Ex06CreateTwinDB
{
    //// 06. Create a database called NorthwindTwin with the same structure as Northwind using 
    //// the features from DbContext. Find for the API for schema generation in MSDN or in Google
    class CreateTwinDB
    {
        static void Main(string[] args)
        {
            NorthwindEntities northwindEntities = new NorthwindEntities();
            CreateTwinDatabase(northwindEntities);
            Console.WriteLine("Twin Database Created!");
        }

        static void CreateTwinDatabase(NorthwindEntities northwindEntities)
        {
            string dbName = northwindEntities.Database.SqlQuery<string>("SELECT DB_NAME() AS Name").First();
            string sqlCreateDBString = "use master \n CREATE DATABASE [" + dbName + "Twin" + "]";

            string sqlSchemaString = GetSchema(northwindEntities);
            sqlSchemaString = sqlSchemaString.Replace(dbName, dbName + "Twin");
            sqlSchemaString = "USE [" + dbName + "Twin" + "] \n " + sqlSchemaString;

            SqlConnection newDbConnection = new SqlConnection("Server=.; Database=master; Integrated Security=true");
            newDbConnection.Open();

            using (newDbConnection)
            {
                SqlCommand cmdCreateDB = new SqlCommand(sqlCreateDBString, newDbConnection);
                cmdCreateDB.ExecuteNonQuery();

                SqlCommand cmdCreateSchema = new SqlCommand(sqlSchemaString, newDbConnection);
                cmdCreateSchema.ExecuteNonQuery();

                newDbConnection.Close();
            }
        }

        static string GetSchema(NorthwindEntities northwindEntities)
        {
            IObjectContextAdapter schema = northwindEntities;
            string sqlSchema = schema.ObjectContext.CreateDatabaseScript();
            return sqlSchema;
        }
    }
}