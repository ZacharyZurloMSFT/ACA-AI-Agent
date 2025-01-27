namespace ZurlocloudRunningWebAPI.Entities;
public class Orders
{
    public required int OrderID { get; set; }
    public required int CustomerID { get; set; }
    public required int StoreID { get; set; }
    public required DateTime OrderDate { get; set; }
}
