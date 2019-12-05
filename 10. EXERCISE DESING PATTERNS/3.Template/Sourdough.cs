
namespace _3.Template
{
    public class Sourdough : Bread
    {
        public override void Bake()
        {
            System.Console.WriteLine($"Baking the Sourdough Bread. (20 minutes)");
        }

        public override void MixIngredients()
        {
            System.Console.WriteLine($"Gathering Ingredients for Sourdough Bread.");
        }
    }
}
