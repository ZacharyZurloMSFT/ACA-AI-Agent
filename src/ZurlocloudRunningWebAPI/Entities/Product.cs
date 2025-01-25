namespace ZurlocloudRunningWebAPI.Entities;
public class Product
{
    public required int ProductID { get; set; }
    public required int ProductName { get; set; }
    public required string ProductCategory { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}
