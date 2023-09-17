// https://github.com/Azure/azure-functions-openapi-extension/blob/main/docs/enable-open-api-endpoints-out-of-proc.md

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
    .Build();

host.Run();
