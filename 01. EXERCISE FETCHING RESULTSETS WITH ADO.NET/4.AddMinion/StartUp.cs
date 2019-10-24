using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _4.AddMinion
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        public static void Main(string[] args)
        {
            List<string> inputInfoMinnion = Console.ReadLine().Split().ToList();
            string nameMinnion = inputInfoMinnion[1];
            int age = int.Parse(inputInfoMinnion[2]);
            string town = inputInfoMinnion[3];

            int townId = 0;
            int villainId = 0;
            int minionId = 0;

            List<string> inputInfoVillain = Console.ReadLine().Split().ToList();
            string nameVillain = inputInfoVillain[1];

            string sqlFindTownId = "SELECT Id FROM Towns WHERE Name = @townName";
            string sqlFindVillain = "SELECT Id FROM Villains WHERE Name = @Name";
            string sqlFindMinion = "SELECT Id FROM Minions WHERE Name = @Name";
            string sqlMinionsVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand commandFindTown = new SqlCommand(sqlFindTownId, connection))
                {
                    commandFindTown.Parameters.AddWithValue("@townName", town);

                    if (commandFindTown.ExecuteScalar() == null)
                    {                      
                        string sqlInsertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

                        using (SqlCommand commandAddTown = new SqlCommand(sqlInsertTown, connection))
                        {
                            commandAddTown.Parameters.AddWithValue("@townName", town);

                            commandAddTown.ExecuteNonQuery();

                            Console.WriteLine($"Town {town} was added to the database.");
                        }                      
                    }

                    townId = (int)commandFindTown.ExecuteScalar();
                }


                using (SqlCommand commandFindVillain = new SqlCommand(sqlFindVillain, connection))
                {
                    commandFindVillain.Parameters.AddWithValue("@Name", nameVillain);

                    if (commandFindVillain.ExecuteScalar() == null)
                    {
                        string sqlInsertVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

                        using (SqlCommand commandAddVillain = new SqlCommand(sqlInsertVillain, connection))
                        {
                            commandAddVillain.Parameters.AddWithValue("@villainName", nameVillain);

                            commandAddVillain.ExecuteNonQuery();

                            Console.WriteLine($"Villain {nameVillain} was added to the database.");
                        }                     
                    }

                    villainId = (int)commandFindVillain.ExecuteScalar();
                }


                using (SqlCommand commandFindMinion = new SqlCommand(sqlFindMinion, connection))
                {
                    commandFindMinion.Parameters.AddWithValue("@Name", nameMinnion);

                    if (commandFindMinion.ExecuteScalar() == null)
                    {
                        string sqlInsertMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

                        using (SqlCommand commandAddMinion = new SqlCommand(sqlInsertMinion, connection))
                        {
                            commandAddMinion.Parameters.AddWithValue("@nam", nameMinnion);
                            commandAddMinion.Parameters.AddWithValue("@age", age);
                            commandAddMinion.Parameters.AddWithValue("@townId", townId);

                            commandAddMinion.ExecuteNonQuery();
                        }
                    }

                    minionId = (int)commandFindMinion.ExecuteScalar();
                }


                using (SqlCommand commandMinionsVillain = new SqlCommand(sqlMinionsVillain, connection))
                {
                    commandMinionsVillain.Parameters.AddWithValue("@villainId",  villainId);
                    commandMinionsVillain.Parameters.AddWithValue("@minionId", minionId);

                    commandMinionsVillain.ExecuteNonQuery();
                }

                Console.WriteLine($"Successfully added { nameMinnion} to be minion of {nameVillain}.");

                connection.Close();
            }
        }
    }
}
