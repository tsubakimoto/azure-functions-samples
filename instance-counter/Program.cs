// https://github.com/Azure/azure-functions-openapi-extension/blob/main/docs/enable-open-api-endpoints-out-of-proc.md
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    //.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
    .Build();

host.Run();
