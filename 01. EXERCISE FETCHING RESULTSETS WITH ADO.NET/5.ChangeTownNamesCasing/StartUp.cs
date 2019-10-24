using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5.ChangeTownNamesCasing
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        public static void Main(string[] args)
        {
            string country = Console.ReadLine();

            string sqlFindTowns = @"SELECT t.Name 
                           FROM Towns as t
                           JOIN Countries AS c ON c.Id = t.CountryCode
                           WHERE c.Name = @countryName";

            string sqlUpdateTownNames = @"UPDATE Towns
                           SET Name = UPPER(Name)
                         WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand commandUpdateNames = new SqlCommand(sqlUpdateTownNames, connection))
                {
                    commandUpdateNames.Parameters.AddWithValue("@countryName", country);

                    commandUpdateNames.ExecuteNonQuery();
                }

                List<string> towns = new List<string>();

                using (SqlCommand commandFindTowns = new SqlCommand(sqlFindTowns, connection))
                {
                    commandFindTowns.Parameters.AddWithValue("@countryName", country);

                    using (SqlDataReader reader = commandFindTowns.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            towns.Add((string)reader[0]);
                        }
                    }
                }

                if(towns.Count == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    Console.WriteLine($"{towns.Count} town names were affected. ");
                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }

                connection.Close();
            }
        }
    }
}
