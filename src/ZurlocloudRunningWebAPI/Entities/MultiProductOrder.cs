namespace ZurlocloudRunningWebAPI.Entities;
public class MultiProductOrder
{
    public required int OrderID { get; set; }
    public required int CustomerID { get; set; }
    public required int StoreID { get; set; }
    public required DateTime OrderDate { get; set; }
    public required int ProductCount { get; set; }
}
