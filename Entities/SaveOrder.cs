 namespace NikuAPI.Entities;

public class SaveOrder
{
    public Guid OrderID { get; set; }
    public int OrderNumber { get; set; }
    public string Number { get; set; }
    public string GoodTypeID { get; set; }
    public string GoodID { get; set; }
    public string? ScoopID { get; set; }
    public string Price { get; set; }
    public string? BespokenID { get; set; }
    public string? PackingID { get; set; }
    public string? DiscountPers { get; set; }
    public string? Taxes { get; set; }
    public string? StoreIDs { get; set; }
    public long Tax { get; set; }
    public string? Mobile { get; set; }
    public int OrderTypeID { get; set; }
    public bool PayBill { get; set; }
    public string? MelliCode { get; set; }
    public string? Address { get; set; }
    public int? CourierID { get; set; }
    public int? NetID { get; set; }
    public string? CustomerName { get; set; }
    public long Discount { get; set; }
    public int DeliveryTypeID { get; set; }
    public int? SaloonID { get; set; }
    public int? CustomerCode { get; set; }
    public int PersonalID { get; set; }
    public int? PagerNumber { get; set; }
    public int? StoreID { get; set; }
    public string Payments { get; set; }
    public int CourierCost { get; set; }
    public int? CompanyID { get; set; }
    public int? MarketerCode { get; set; }
    public int? MarketerShare { get; set; }
    public bool UpdateDelivery { get; set; }
    public bool Reserved { get; set; }
    public int? LedgerID { get; set; }
    public int? GuestNumber { get; set; }
    public string? Description { get; set; }
    public int OrderDate { get; set; }
}

public class OrderResponse
{
    public int OrderNumber { get; set; }
    public string OrderTime { get; set; }
    public Guid OrderID { get; set; }
    //public float WinDiscount { get; set; }
    public string OrderDate { get; set; }
    //public int DayofWeek { get; set; }
    //public int OrderNumber_2 { get; set; }
    //public int OrderNumber_Month { get; set; }
    //public int OrderNumber_Year { get; set; }
}
