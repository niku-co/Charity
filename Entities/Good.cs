using System.ComponentModel.DataAnnotations.Schema;

namespace NikuAPI.Entities;

public class Good
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Topic { get; set; }
    public long Price { get; set; }
    public bool Active { get; set; }
    public int AccountIndex { get; set; }
    public int LayoutIndex { get; set; }
    public string ChildIDs { get; set; }
    public string Unit { get; set; }

    public int UnitID { get; set; }
    public string Description { get; set; }
    public long OldPrice { get; set; }

    [NotMapped]
    public Good Parent { get; set; }

    private List<Good> _subProducts = new();
    [NotMapped]
    public List<Good> SubProducts { get => _subProducts; set => _subProducts = value; }

    private bool _stock = true;
    [NotMapped]
    public bool Stock { get => _stock; set => _stock = value; }
}

public class GoodImage
{
    public int GoodID { get; set; }
    public byte[] Ima { get; set; }
    public byte[] Ima_Large { get; set; }
    public string LastImageUpdate { get; set; }
}

public class OrderGood
{
    public int GoodID { get; set; }
    public string Topic { get; set; }
    public string Numbers { get; set; }
}