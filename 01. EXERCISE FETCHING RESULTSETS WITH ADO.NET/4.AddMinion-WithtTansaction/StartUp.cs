using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _4.AddMinion_WithtTansaction
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlTransaction transaction;
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
            string sqlInsertTown = "INSERT INTO Towns (Name) VALUES (@townName)";
            string sqlFindVillain = "SELECT Id FROM Villains WHERE Name = @Name";
            string sqlInsertVillain = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
            string sqlInsertMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
            string sqlFindMinion = "SELECT Id FROM Minions WHERE Name = @Name";
            string sqlMinionsVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = sqlFindTownId;
                    command.Parameters.AddWithValue("@townName", town);

                    using (command)
                    {
                        if (command.ExecuteScalar() == null)
                        {
                            command.CommandText = sqlInsertTown;

                            command.ExecuteNonQuery();

                            Console.WriteLine($"Town {town} was added to the database.");
                        }

                        command.CommandText = sqlFindTownId;
                        townId = (int)command.ExecuteScalar();

                        command.CommandText = sqlFindVillain;

                        command.Parameters.AddWithValue("@Name", nameVillain);

                        if (command.ExecuteScalar() == null)
                        {
                            command.CommandText = sqlInsertVillain;

                            command.Parameters.AddWithValue("@villainName", nameVillain);

                            command.ExecuteNonQuery();

                            Console.WriteLine($"Villain {nameVillain} was added to the database.");
                        }

                        command.CommandText = sqlFindVillain;
                        villainId = (int)command.ExecuteScalar();

                        command.CommandText = sqlFindMinion;

                        command.Parameters["@Name"].Value = nameMinnion;

                        if (command.ExecuteScalar() == null)
                        {
                            command.CommandText = sqlInsertMinion;

                            command.Parameters.AddWithValue("@nam", nameMinnion);
                            command.Parameters.AddWithValue("@age", age);
                            command.Parameters.AddWithValue("@townId", townId);

                            command.ExecuteNonQuery();
                        }

                        command.CommandText = sqlFindMinion;
                        minionId = (int)command.ExecuteScalar();

                        command.CommandText = sqlMinionsVillain;

                        command.Parameters.AddWithValue("@villainId", villainId);
                        command.Parameters.AddWithValue("@minionId", minionId);

                        command.ExecuteNonQuery();

                        transaction.Commit();

                        Console.WriteLine($"Successfully added { nameMinnion} to be minion of {nameVillain}.");
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (Exception re)
                    {
                        Console.WriteLine(re.Message);
                    }
                }


                connection.Close();
            }
        }
    }
}

