
namespace _3.Template
{
    public class TwelveGrain : Bread
    {
        public override void Bake()
        {
            System.Console.WriteLine($"Baking the 12-Grain Bread. (25 minutes)");
        }

        public override void MixIngredients()
        {
            System.Console.WriteLine($"Gathering Ingredients for 12-Grain Bread.");
        }
    }
}
