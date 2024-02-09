namespace Common.Core.ConsoleApp.Test
{
    // Using Simple Injector
    public class MainActivityDecorator: MainActivity
    {
        public MainActivityDecorator(MainActivity mainActivity)
        {
        }

        public override int Print()
        {
            Console.WriteLine("MainActivityDecorator.Before.Print");
            base.Print();
            Console.WriteLine("MainActivityDecorator.After.Print");
            return 0;
        }
    }
}
