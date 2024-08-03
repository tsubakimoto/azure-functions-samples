namespace AzureSqlBindingsSample.Models;

public class Product
{
    public string ProductID { get; set; }
    public string Name { get; set; }
    public string ProductNumber { get; set; }
    public string Color { get; set; }
    public string StandardCost { get; set; }
    public string ListPrice { get; set; }
    public string Size { get; set; }
    public string Weight { get; set; }
    public string ProductCategoryID { get; set; }
    public string ProductModelID { get; set; }
    public DateTime SellStartDate { get; set; }
    public object SellEndDate { get; set; }
    public object DiscontinuedDate { get; set; }
    public string ThumbNailPhoto { get; set; }
    public string ThumbnailPhotoFileName { get; set; }
    public string rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}
