using P01_StudentSystem.Data;
using P01_StudentSystem.Initializer;


namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new StudentSystemContext();

            DbInitializer initializer = new DbInitializer();
            initializer.ResetDatabase(context);
        }
    }
}
