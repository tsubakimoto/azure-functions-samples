namespace AzureSqlBindingsSample.Models;

public class ProductCategory
{
    public int ProductCategoryID { get; set; }
    public int? ParentProductCategoryID { get; set; } = null;
    public string Name { get; set; }
    public string rowguid { get; set; }
}
