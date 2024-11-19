namespace NikuAPI.Entities;

public class Order
{
    public Guid OrderID { get; set; }
    public int OrderNumber { get; set; }
    public string OrderDate { get; set; }
    public string GoodIDs { get; set; }
    public string Numbers { get; set; }
    public string? DeliveryTime { get; set; }
    public bool PayBill { get; set; }
    public bool Cancel {  get; set; }
    public bool Reserved { get; set; }
}
