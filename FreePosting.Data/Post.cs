using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreePosting.Data
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

    }
    

    public class PostDb
    {
        private string _Constr;
        public PostDb(string Constr)
        {
            _Constr = Constr;
        }
        public IEnumerable<Post> GetPosts()
        {
            using (SqlConnection connection = new SqlConnection(_Constr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * From FreePosting ORder By DateSubmitted Desc";
                connection.Open();
                List<Post> posts = new List<Post>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    object value = reader["Name"];
                    var post = new Post
                    {
                        Id = (int)reader["Id"],
                        DateSubmitted = (DateTime)reader["DateSubmitted"],
                        Text = (string)reader["Text"],
                        Number = (string)reader["Number"],
                    };
                    if (value != DBNull.Value)
                    {
                        post.Name = (string)value;
                    }
                    posts.Add(post);

                }
                return posts;
            }
        }
        public int InsertPost(Post Post)
        {
            using (SqlConnection connection = new SqlConnection(_Constr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO FreePosting (Name, Number, Text, DateSubmitted) " +
                    "Values (@Name, @Number, @Text, GetDate()) SELECT CAST(SCOPE_IDENTITY() AS INT)";

                object name = Post.Name;
                if (name == null)
                {
                    name = DBNull.Value;
                }
                command.Parameters.AddWithValue("@Number", Post.Number);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Text", Post.Text);
                connection.Open();
                Post.Id = (int)command.ExecuteScalar();
                connection.Close();
                connection.Dispose();
                return Post.Id;


            }
        }
        public void Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_Constr))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "Delete From FreePosting Where Id = @Id";
                connection.Open();
                command.Parameters.AddWithValue("Id", @Id);
                command.ExecuteNonQuery();
            }

        }
    }
}
