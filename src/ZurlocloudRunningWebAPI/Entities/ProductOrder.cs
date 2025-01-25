namespace ZurlocloudRunningWebAPI.Entities;
public class ProductOrder
{
    public required int ProductOrderID { get; set; }
    public required int OrderID { get; set; }
    public required int ProductID { get; set; }
    public required int Quantity { get; set; }
}
