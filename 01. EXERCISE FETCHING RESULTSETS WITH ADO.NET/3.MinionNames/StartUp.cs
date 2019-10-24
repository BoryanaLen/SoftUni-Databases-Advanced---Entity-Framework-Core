using System;
using System.Data.SqlClient;

namespace _3.MinionNames
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int id = int.Parse(Console.ReadLine());

                string sqlQueryName = "SELECT Name FROM Villains WHERE Id = @id";

                string sqlQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(sqlQueryName, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    string name = (string)command.ExecuteScalar();

                    if (name == null)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {name}");
                }


                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long rowNumber = (long)reader[0];
                            string name = (string)reader[1];
                            int age = (int)reader[2];

                            Console.WriteLine($"{rowNumber}. {name} {age}");
                        }

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("no minions");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
