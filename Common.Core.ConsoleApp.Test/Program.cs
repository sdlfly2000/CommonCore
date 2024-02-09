using SimpleInjector;

namespace Common.Core.ConsoleApp.Test;

public class Program
{
    static readonly Container container;

    static Program()
    {
        container = new Container();

        container.Register<MainActivity>();
        container.RegisterDecorator<MainActivity, MainActivityDecorator>();
    }

    public static void Main(string[] args)
    {
        var mainActivity = container.GetInstance<MainActivity>();
        mainActivity.Print();
    }
}