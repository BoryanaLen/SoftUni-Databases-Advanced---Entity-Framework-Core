using System;
using System.Data.SqlClient;

namespace _9.IncreaseAgeStoredProcedure
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
        public static void Main(string[] args)
        {

            int minionId = int.Parse(Console.ReadLine());

            string sqlUpdate = @"EXEC dbo.usp_GetOlder @id";

            string sqlSelectFromMinions = @"SELECT Name, Age FROM Minions WHERE Id = @Id";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand commandUpdate = new SqlCommand(sqlUpdate, connection))
                {
                    commandUpdate.Parameters.AddWithValue("@id", minionId);

                    commandUpdate.ExecuteNonQuery();
                }

                using (SqlCommand commandSelect = new SqlCommand(sqlSelectFromMinions, connection))
                {
                    commandSelect.Parameters.AddWithValue("@Id", minionId);

                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{(string)reader[0]} – {(int)reader[1]} years old");
                        }
                    }
                }


                connection.Close();
            }           
        }
    }
}
