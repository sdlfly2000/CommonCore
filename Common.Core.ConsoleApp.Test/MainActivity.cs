using Common.Core.AOP.Cache;

namespace Common.Core.ConsoleApp.Test
{
    public class MainActivity
    {
        private readonly IServiceProvider _serviceProvider;

        public MainActivity(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Cache(masterKey: "Model", returnType: typeof(ModelReturn), cachedTypes: typeof(Model))]
        public async Task<ModelReturn> Print(Model model, CancellationToken token)
        {
            Console.WriteLine("MainActivity.Print");
            return new ModelReturn
            {
                Name = "test",
                Age = 18
            };
        }
    }
}
