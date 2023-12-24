using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BookSearchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly string connectionString = "Data Source=VENKATESWARLU\\SQLEXPRESS;Initial Catalog=Book;Integrated Security=True;TrustServerCertificate=True";

        [HttpGet]
        public ActionResult<IEnumerable<Book>> SearchBooks(string query, int page = 1, int pageSize = 10)
        {
            using IDbConnection dbConnection = new SqlConnection(connectionString);

            // Implement search query based on title, author, description
            var searchQuery = "%" + query + "%";
            var sql = @"
                SELECT * FROM Books
                WHERE Title LIKE @searchQuery OR Author LIKE @searchQuery
                OR Description LIKE @searchQuery
                ORDER BY Id
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
            ";

            var offset = (page - 1) * pageSize;

            var books = dbConnection.Query<Book>(sql, new { searchQuery, offset, pageSize });

            return Ok(books);
        }
    }

}
