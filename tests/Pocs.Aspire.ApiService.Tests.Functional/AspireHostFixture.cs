using Aspire.Hosting;
using LanguageExt.ClassInstances;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace Pocs.Aspire.ApiService.Tests.Functional;

public class AspireHostFixture : IAsyncLifetime
{
    public DistributedApplication App { get; private set; } = default!;
    public HttpClient HttpClient { get; private set; } = default!;


    public async ValueTask InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Pocs_Aspire_AppHost>(
               [],  (app, settings) => {
                app.EnableResourceLogging = true;
             }, default);


        //Logger.WriteLine($"Initializing tests");
        
        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        App = await builder.BuildAsync();
        await App.StartAsync();
        
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        await App.ResourceNotifications
          .WaitForResourceHealthyAsync("apiservice", cts.Token)
          .WaitAsync(cts.Token);


        HttpClient = App.CreateHttpClient("apiservice");

        //Logger.WriteLine($"API listening on {HttpClient.BaseAddress}");
      
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await App.StopAsync();
        await App.DisposeAsync();
    }
}
