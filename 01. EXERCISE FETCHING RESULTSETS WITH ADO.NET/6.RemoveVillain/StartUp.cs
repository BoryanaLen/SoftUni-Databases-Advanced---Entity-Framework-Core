using System;
using System.Data.SqlClient;

namespace _6.RemoveVillain
{
    public class StartUp
    {
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";

        private static SqlTransaction transaction;

        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            string sqlFindVillainName = @"SELECT Name FROM Villains WHERE Id = @villainId";

            string sqlDeleteMinionsVillains = @"DELETE FROM MinionsVillains
                                        WHERE VillainId = @villainId";
            string sqlDeleteVillain = @"DELETE FROM Villains
                                    WHERE Id = @villainId";


            string VillainName = "";
            int numberMinions = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = sqlFindVillainName;
                    command.Parameters.AddWithValue("@villainId", villainId);

                    using (command)
                    {
                        object value = command.ExecuteScalar();

                        if (value == null)
                        {
                            throw new ArgumentException("No such villain was found.");
                        }

                        VillainName = (string)value;

                        command.CommandText = sqlDeleteMinionsVillains;

                        numberMinions = command.ExecuteNonQuery();

                        command.CommandText = sqlDeleteVillain;

                        command.ExecuteNonQuery();

                        transaction.Commit();

                        Console.WriteLine($"{VillainName} was deleted.");
                        Console.WriteLine($"{numberMinions} minions were released.");
                    }
                }
                catch (ArgumentException ae)
                {
                    try
                    {
                        Console.WriteLine(ae.Message);
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
