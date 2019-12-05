
namespace _3.Template
{
    public class WholeWheat : Bread
    {
        public override void Bake()
        {
            System.Console.WriteLine($"Baking the Whole Wheat Bread. (15 minutes)");
        }

        public override void MixIngredients()
        {
            System.Console.WriteLine($"Gathering Ingredients for Whole Wheat Bread.");
        }
    }
}
