﻿using Aspire.Hosting;
namespace Pocs.Aspire.ApiService.Tests.Functional;

public class AspireHostFixture : IAsyncLifetime
{
    public DistributedApplication App { get; private set; } = default!;
    public HttpClient HttpClient { get; private set; } = default!;

    public async ValueTask InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Pocs_Aspire_AppHost>();

        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        App = await builder.BuildAsync();
        await App.StartAsync();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await App.ResourceNotifications.WaitForResourceHealthyAsync("apiservice", cts.Token);

        HttpClient = App.CreateHttpClient("apiservice");
    }

    public async ValueTask DisposeAsync()
    {
        await App.StopAsync();
        await App.DisposeAsync();
    }
}
