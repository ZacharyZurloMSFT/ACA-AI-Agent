namespace ZurlocloudRunningWebAPI.Entities;
public class StoreProducts
{
    public required int StoreID { get; set; }
    public required int ProductID { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}
