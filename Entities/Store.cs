using System.ComponentModel.DataAnnotations.Schema;

namespace NikuAPI.Entities;

public class Store
{
    public string StoreName { get; set; }
    public byte[] StoreLogo { get; set; }
    public byte[] SMS { get; set; }
    public string SMSReady { get; set; }
    public int LastNetID { get; set; }
    public string SMSSell { get; set; }
    public string FishMessage { get; set; }
    public int AddedTime { get; set; }
    public byte[] PrintSetting { get; set; }
    public byte[] OrderSetting { get; set; }
    public string KioskFishMessage { get; set; }
    public string StoreName_En { get; set; }
    public string FishMessage_En { get; set; }
    public string KioskFishMessage_En { get; set; }
    public int TotalDiscount { get; set; }
    public int KioskDiscount { get; set; }
    public string CourierFishMessage { get; set; }
    public byte[] StoreVal { get; set; }
    public byte[] StartVal { get; set; }
    public string SMSCourier { get; set; }
    public string SMSPost { get; set; }
    public byte[] HolidayMode { get; set; }
    public byte[] NormalMode { get; set; }
    public int Mode { get; set; }
    public int LedgerIDTax { get; set; }
    public int LedferIDTax_P { get; set; }
    public byte[] DiscountPlan { get; set; }
    public string CourierFishMessage_En { get; set; }
    public string StoreName_Ku { get; set; }
    public string FishMessage_Ku { get; set; }
    public string KioskFishMessage_Ku { get; set; }
    public string CourierFishMessage_Ku { get; set; }
    public string StoreName_Tr { get; set; }
    public string FishMessage_Tr { get; set; }
    public string KioskFishMessage_Tr { get; set; }
    public string CourierFishMessage_Tr { get; set; }
    public string StoreName_Ar { get; set; }
    public string FishMessage_Ar { get; set; }
    public string KioskFishMessage_Ar { get; set; }
    public string CourierFishMessage_Ar { get; set; }
    public string SMSTableEmpty { get; set; }
    public int StoreID { get; set; }
    public Guid MyCustomerID { get; set; }
    public string KioskDisplayMessage { get; set; }
    public ModeResult ModeResult = new();

    private string _smsStructure;
    private string _code;
    private string _userName;
    private string _password;
    private string _company;
    private int _lineType;

    [NotMapped]
    public string SmsStructure { get { return System.Text.Encoding.UTF8.GetString(SMS); ; } set => _smsStructure = value; }
    [NotMapped]
    public string Code { get { return SMS.Length > 0 ? SmsStructure.Split(',')[0] : ""; } set => _code = value; }
    [NotMapped]
    public string UserName { get { return SMS.Length > 1 ? SmsStructure.Split(',')[1] : ""; } set => _userName = value; }
    [NotMapped]
    public string Password { get { return SMS.Length > 2 ? SmsStructure.Split(',')[2] : ""; } set => _password = value; }
    [NotMapped]
    public string Company { get { return SMS.Length > 3 ? SmsStructure.Split(',')[3] : "0"; } set => _company = value; }
    [NotMapped]
    public int LineType { get { return SMS.Length > 3 ? int.Parse(SmsStructure.Split(',')[3]) : 1; } set => _lineType = value; }
}

public class ModeResult
{
    public long Personal { get; set; }
    public long Away { get; set; }
    public long Courier { get; set; }
    public long Post { get; set; }
}

public class StoreId
{
    public int Id { get; set; }
}