using Common.Core.AOP;
using Common.Core.DependencyInjection;
using Common.Core.LogService;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddLogging((configure) => configure.AddProvider(new Log2FileProvider(builder.Configuration)))
    .RegisterDomain("Common.Core.AspNet.Test")
    .RegisterInterceptor(enableLog: false, enableCache: true, null, "Common.Core.AspNet.Test");

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();