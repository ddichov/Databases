using System;
using MySql.Data.MySqlClient;

namespace MySQL_Connection
{
    class MySQLConn
    {
        static void Main(string[] args)
        {
            MySqlConnection mySQLConn = new MySqlConnection(
                "Server=localhost;Port=3306;Database=Bookstore;Uid=root;Pwd=EnterPassword;");
            mySQLConn.Open();

            using (mySQLConn)
            {
                AddBook(mySQLConn);
                FindBookByName(mySQLConn);
                ListBooks(mySQLConn);
            }
        }

        private static void AddBook(MySqlConnection mySQLConn)
        {
            Console.Write("Enter new book's title : ");
            string title = Console.ReadLine();

            Console.Write("Enter new book's author : ");
            string author = Console.ReadLine();

            DateTime publishDate = DateTime.Now;

            Console.Write("Enter new book's ISBN : ");
            int ISBN = int.Parse(Console.ReadLine());

            MySqlCommand searchQuery =
                new MySqlCommand("SELECT AuthorId FROM authors WHERE Name = @newAuthor;", mySQLConn);
            MySqlParameter searchTermParam = new MySqlParameter("@newAuthor", author);
            searchQuery.Parameters.Add(searchTermParam);

            MySqlDataReader reader = searchQuery.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();

                MySqlCommand insertQuery =
                    new MySqlCommand("INSERT INTO authors (Name) VALUES(@newAuthor);", mySQLConn);
                MySqlParameter insertParam = new MySqlParameter("@newAuthor", author);
                insertQuery.Parameters.Add(insertParam);
                insertQuery.ExecuteNonQuery();

                reader = searchQuery.ExecuteReader();
            }

            reader.Read();
            int authorId = (int)reader["AuthorId"];

            reader.Close();

            MySqlCommand insertBookQuery =
                new MySqlCommand("INSERT INTO books (Title, AuthorId, PublishDate, ISBN) VALUES(@title, @authorId, @publishDate, @ISBN);", mySQLConn);
            insertBookQuery.Parameters.Add(new MySqlParameter("@title", title));
            insertBookQuery.Parameters.Add(new MySqlParameter("@authorId", authorId));
            insertBookQuery.Parameters.Add(new MySqlParameter("@publishDate", publishDate));
            insertBookQuery.Parameters.Add(new MySqlParameter("@ISBN", ISBN));
            insertBookQuery.ExecuteNonQuery();
        }

        private static void FindBookByName(MySqlConnection mySQLConn)
        {
            Console.Write("Enter book title to find :");
            string title = Console.ReadLine();

            MySqlCommand searchQuery = 
                new MySqlCommand("SELECT b.Title, a.Name as Author FROM books b join authors a on b.AuthorId = a.AuthorId WHERE b.Title LIKE @searchTerm", mySQLConn);
            MySqlParameter searchTermParam = new MySqlParameter("@searchTerm", '%' + title + '%');
            searchQuery.Parameters.Add(searchTermParam);

            MySqlDataReader reader = searchQuery.ExecuteReader();

            using (reader)
            {
                Console.WriteLine(Environment.NewLine + "Books Found:");

                while (reader.Read())
                {
                    string book = string.Format("{0} by {1}", (string)reader["Title"], (string)reader["Author"]);
                    Console.WriteLine(book);
                }
            }
        }

        private static void ListBooks(MySqlConnection mySQLConn)
        {
            MySqlCommand getBooks = new MySqlCommand(
                "SELECT b.Title, a.Name as Author, b.PublishDate, b.ISBN from books b join authors a on b.AuthorID = a.AuthorID",
                mySQLConn);

            MySqlDataReader listBooks = getBooks.ExecuteReader();

            while (listBooks.Read())
            {
                Console.WriteLine("Title : {0}", (string)listBooks["Title"]);
                Console.WriteLine("Author : {0}", (string)listBooks["Author"]);
                Console.WriteLine("Publish date : {0}", (DateTime)listBooks["PublishDate"]);
                Console.WriteLine("ISBN : {0}", (int)listBooks["ISBN"]);
                Console.WriteLine();
            }
        }
    }
}
