using GeeksForLessAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace GeeksForLessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HierarchyController : ControllerBase
    {
        string connectionString = "Server=DESKTOP-9QQ3N39;Database=GeeksForLess;Trusted_Connection=True;";
        string initial = "Creating Digital Images";
        List<FlattenCategories> list = new List<FlattenCategories>();
        Dictionary<int, int> parentChildMap = new Dictionary<int, int>();
        int id = 1;

        [HttpGet("GetHierarchy")]
        public async Task<IActionResult> GetHierarchy()
        {
            //await GetCategoriesAsync("Creating Digital Images");
            string name = await GetFirstCategoryAsync();
            await GetOriginalCategoriesAsync(name);
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost("AddHierarchy")]
        public async Task<IActionResult> AddHierarchy(string path = "D:\\CreatingDigitalImages")
        {
            //string path = model.Path;
            var temp = path.Split('\\');
            //initial = temp[temp.Length - 1];
            var textBox1 = new { Text = path };
            var directory = new DirectoryInfo(textBox1.Text);
            
            var items = new List<HierarchicalItemCopy>();
            try
            {
                SearchDirectoryListCopy(directory, items);
                await TruncateCategoriesTableAsync(connectionString);
                foreach (var item in items)
                {
                    InsertItemToDatabase(connectionString, item);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [NonAction]
        async Task GetOriginalCategoriesAsync(string value)
        {
            string sqlExpression = "GetCategoriesAdjacent";
            string sqlExpressionForCategories = "WITH [RecursiveCategories] AS (\r\n   SELECT [Id], [Category], [ParentId]\r\n   FROM [Categories]\r\n   WHERE [Category] = @CategoryName\r\n   UNION ALL\r\n   SELECT c.[Id], c.[Category], c.[ParentId]\r\n   FROM [Categories] c\r\n   JOIN [RecursiveCategories] rc ON c.[ParentId] = rc.[Id]\r\n   )\r\n   SELECT [Id], [Category], [ParentId]\r\n   FROM [RecursiveCategories]\r\n   order by ParentId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpressionForCategories, connection);
                command.Parameters.AddWithValue("@CategoryName", value);
                //command.CommandType = CommandType.StoredProcedure;
                command.CommandType = CommandType.Text;
                //SqlParameter sqlParameter = new SqlParameter
                //{
                //    ParameterName = "@filter",
                //    SqlDbType = SqlDbType.VarChar,
                //    Value = value,
                //    Direction = ParameterDirection.Input
                //};
                //command.Parameters.Add(sqlParameter);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            int categoryId = reader.GetInt32(0);
                            string categoryName = reader.GetString(1);
                            int parentId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                            FlattenCategories category = new FlattenCategories { Id = categoryId, Name = categoryName, ParentId = parentId };
                            list.Add(category);
                        }

                    }
                }
            }
        }
        [NonAction]
        async Task<string> GetFirstCategoryAsync()
        {
            string sqlExpressionForCategories = "select top(1) Category from Categories";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string res = "";
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpressionForCategories, connection);
                command.CommandType = CommandType.Text;
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            res = reader.GetString(0);
                        }

                    }
                }
                return res;
            }
        }

        [NonAction]
        static void InsertItemToDatabase(string connectionString, HierarchicalItemCopy item)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Categories (Category, ParentId) VALUES (@Category, @ParentId);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Category", item.Name);
                    if (item.ParentId == 0)
                    {
                        command.Parameters.AddWithValue("@ParentId", (object)DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ParentId", item.ParentId);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        [NonAction]
        static async Task TruncateCategoriesTableAsync(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string truncateSql = "TRUNCATE TABLE Categories;";
                using (SqlCommand truncateCommand = new SqlCommand(truncateSql, connection))
                {
                    await truncateCommand.ExecuteNonQueryAsync();
                }
            }
        }


        //[NonAction]
        //static void SearchDirectoryList(DirectoryInfo directory, List<HierarchicalItem> result, int deep = 0, int id = 1)
        //{
        //    result.Add(new HierarchicalItem(id, directory.Name, deep));

        //    foreach (var subdirectory in directory.GetDirectories())
        //    {
        //        SearchDirectoryList(subdirectory, result, deep + 1, id + 1);
        //    }
        //}
        [NonAction]
        void SearchDirectoryListCopy(DirectoryInfo directory, List<HierarchicalItemCopy> result, int parentId = 0, int depth = 0)
        {
            var currentItem = new HierarchicalItemCopy(id, directory.Name, parentId, depth);
            result.Add(currentItem);

            parentChildMap[id] = parentId;

            var currentId = id;
            id++;

            foreach (var subdirectory in directory.GetDirectories())
            {
                SearchDirectoryListCopy(subdirectory, result, currentId, depth + 1);
            }
        }
    }
   
}
