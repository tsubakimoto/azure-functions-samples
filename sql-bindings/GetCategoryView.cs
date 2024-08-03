namespace AzureSqlBindingsSample;

public class GetCategoryView
{
    private readonly ILogger<GetCategoryView> _logger;

    public GetCategoryView(ILogger<GetCategoryView> log)
    {
        _logger = log;
    }

    [FunctionName("GetCategoryView")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [Sql(
            commandText: "select * from SalesLT.vGetAllCategories where ProductCategoryName = @Name",
            connectionStringSetting: "SqlConnectionString",
            commandType: System.Data.CommandType.Text,
            parameters: "@Name={Query.name}")] IEnumerable<CategoryView> categories,
        [Sql(
            commandText: "select * from SalesLT.ProductCategory where Name = @Name",
            connectionStringSetting: "SqlConnectionString",
            commandType: System.Data.CommandType.Text,
            parameters: "@Name={Query.name}")] IEnumerable<ProductCategory> productCategories)
    {
        _logger.LogInformation("GetCategoryView is running.");
        foreach (var category in categories)
        {
            category.ProductCategory = productCategories.FirstOrDefault(pc => pc.ProductCategoryID == category.ProductCategoryID);
        }
        return new OkObjectResult(categories);
    }
}

