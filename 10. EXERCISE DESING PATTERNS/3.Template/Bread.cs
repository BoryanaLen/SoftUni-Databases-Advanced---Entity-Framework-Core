
namespace _3.Template
{
    public abstract class Bread
    {
        public abstract void MixIngredients();
        public abstract void Bake();
        public virtual void Slice()
        {
            System.Console.WriteLine($"Slicing the " + GetType().Name + " bread!");
        }

        //Template method
        public void Make()
        {
            MixIngredients();
            Bake();
            Slice();
        }
    }
}
