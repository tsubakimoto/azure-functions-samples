namespace AzureSqlBindingsSample.Models;

public class CategoryView
{
    public string ParentProductCategoryName { get; set; }
    public string ProductCategoryName { get; set; }
    public int ProductCategoryID { get; set; }
    public ProductCategory ProductCategory { get; set; }
}
