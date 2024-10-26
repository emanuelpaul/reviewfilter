using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReviewFilter.ThirdParty.MachineLearning;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMachineLearning("C:\\Dev\\Github\\AI\\reviewfilter\\train-dataset 1.csv");
using IHost host = builder.Build();
host.Start();

using IServiceScope scope = host.Services.CreateScope();

string reviewCG = "New store very clean";
string reviewOR = "nice fresh coffee";
string resultCG = scope.ServiceProvider.GetRequiredService<IMachineLearningService>().Analize(reviewCG);
Console.WriteLine("Result for CG review: " + resultCG);
string resultOR = scope.ServiceProvider.GetRequiredService<IMachineLearningService>().Analize(reviewOR);
Console.WriteLine("Result for OR review: " + resultOR);

