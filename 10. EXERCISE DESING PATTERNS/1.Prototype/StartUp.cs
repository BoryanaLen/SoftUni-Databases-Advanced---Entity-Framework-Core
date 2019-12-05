using System;

namespace _1.Prototype
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SandwichMenu sandwichMenu = new SandwichMenu();

            //Initialize with default sandwiches
            sandwichMenu["BLT"] = new Sandwich("Wheat", "Becon", "", "Lettuce, Tomato");
            sandwichMenu["PB&J"] = new Sandwich("White", "", "", "Peanut Butter, Jelly");
            sandwichMenu["Turkey"] = new Sandwich("Rye", "Turkey", "Swiss", "Lettuce, Onion, Tomato");

            //Add custom sandwiches
            sandwichMenu["LoadedBLT"] = new Sandwich("Wheat", "Turkey, Becon", "American", "Lettuce, Tomato, Onion, Olives");
            sandwichMenu["ThreeMeatCombo"] = new Sandwich("Rye", "Turkey, Ham, Salami", "Provolone", "Lettuce, Onion");
            sandwichMenu["Vegetarian"] = new Sandwich("Wheat", "", "", "Lettuce, Onion, Tomato, Olives, Spinach");

            //Clone this sandwiches
            Sandwich sandwichOne = sandwichMenu["BLT"].Clone() as Sandwich;
            Sandwich sandwichTwo = sandwichMenu["ThreeMeatCombo"].Clone() as Sandwich;
            Sandwich sandwichThree = sandwichMenu["Vegetarian"].Clone() as Sandwich;
        }
    }
}
