namespace NikuAPI.Entities;

public class GoodType
{
    public int GoodTypeID { get; set; }
    public string Topic { get; set; }
    public int CategoryID { get; set; }
    public bool ActiveKiosk { get; set; }
    public int? LayOutID { get; set; }
    public int LayOutIndex { get; set; }
    public byte[]? KioskIDs { get; set; }
    public bool Secondary { get; set; }
    public string? PageManagement { get; set; }
    public string? OfferManagement { get; set; }
    private GoodTypeSettings? _setting;

    public GoodTypeSettings? Setting
    {
        set { _setting = value; }
        get 
        {
            if (Settings == null)  return Setting = null;
            var hex = BitConverter.ToString(Settings).Replace("-", "");
            //string binary = Convert.ToString(settings[0], 2);
            string binarystring = String.Join(String.Empty,
                  hex.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                  )
                ).Substring(0, 8);
            var chars = binarystring.Reverse().Select(i => i).ToArray();
            return Setting = new()
            {
                Payment = chars[0] == '1',
                HasPrice = chars[1] == '1',
                CheckStore = chars[2] == '1',
                CheckPhone = chars[3] == '1',
                HasCoupon = chars[4] == '1',
                General = chars[5] == '1',
                OptionalPrint= chars[6] == '1',
            };
        }
    }

    private byte[]? Settings { get; set; }
    public string? Message { get; set; }
    private bool _visited = true;

    public bool Visited
    {
        get { return _visited; }
        set { _visited = value; }
    }
}
public class GoodTypeSettings
{
    public bool Payment { get; set; }
    public bool HasPrice { get; set; }
    public bool CheckStore { get; set; }
    public bool CheckPhone { get; set; }
    public bool HasCoupon { get; set; }
    public bool General { get; set; }
    public bool OptionalPrint { get; set; }
}

public class GoodTypeImage
{
    public int GoodTypeID { get; set;}
    public byte[] Ima12 { get; set;}
    public string LastImageUpdate { get; set; }
    public byte[] Gif { get; set; }

}