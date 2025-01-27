namespace ZurlocloudRunningWebAPI.Entities;
public class AllProducts
{
    public required int ProductID { get; set; }
    public required string ProductName { get; set; }
    public required string ProductCategory { get; set; }
    public required decimal Price { get; set; }
}
