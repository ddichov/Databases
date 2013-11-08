using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05CategoryImages
{
    // Write a program that retrieves the images for all categories in 
    // the Northwind database and stores them as JPG files in the file system.
    class CategoryImages
    {
        static void Main(string[] args)
        {

            SqlConnection db = new SqlConnection("Server=.; Database=NORTHWND; Integrated Security=true");

            db.Open();

            using (db)
            {
                SqlCommand picturesQuery = new SqlCommand("SELECT Picture, CategoryID, CategoryName FROM Categories", db);

                SqlDataReader reader = picturesQuery.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        // Images handling thanks to http://tihomir.me/tag/northwind and vic.alexiev 

                        string categoryName = (string)reader["CategoryName"];
                        if (categoryName.Contains('/') == true)
                        {
                            categoryName = categoryName.Replace('/', ' ');
                        }
                        byte[] pictureBytes = reader["Picture"] as byte[];

                        MemoryStream stream = new MemoryStream(
                            pictureBytes, 78,
                            pictureBytes.Length - 78);

                        Image image = Image.FromStream(stream);
                        using (image)
                        {
                            image.Save("..\\" + categoryName + ".jpg", ImageFormat.Jpeg);
                        }
                    }

                    Console.WriteLine("Images saved in \\bin folder");
                }
            }
        }
    }
}
