using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _8.IncreaseMinionAge
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
        public static void Main(string[] args)
        {
            List<int> mininsIds = Console.ReadLine().Split().Select(int.Parse).ToList();

            string sqlUpdateMinions = @"UPDATE Minions
                           SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                         WHERE Id = @Id";

            string sqlSelectMinions = @"SELECT Name, Age FROM Minions";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (int id in mininsIds)
                {
                    using (SqlCommand commandUpdateMinions = new SqlCommand(sqlUpdateMinions, connection))
                    {
                        commandUpdateMinions.Parameters.AddWithValue("@Id", id);

                        commandUpdateMinions.ExecuteNonQuery();

                    }
                }


                using (SqlCommand commandSelectMinions = new SqlCommand(sqlSelectMinions, connection))
                {
                    using (SqlDataReader reader = commandSelectMinions.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader[0];
                            int age = (int)reader[1];

                            Console.WriteLine($"{name} {age}");
                        }
                    }
  
                }

                connection.Close();
            }
        }
    }
}
