namespace NikuAPI.Entities;

public class Customer
{
    public string? FullName { get; set; }
    public string? Mobile { get; set; }
    public string? MelliCode { get; set; }
    public string? FatherName { get; set; }
    public int? Age { get; set; }
    public int? CompanyCode { get; set; }
}

public class CustomerCountDTO
{
    public string GoodId { get; set; }
    public string NationalCode { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}