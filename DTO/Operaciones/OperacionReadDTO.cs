public class OperacionReadDTO
{
    public int OperacionID { get; set; }
    public int ClienteID { get; set; }
    public string CriptoCode { get; set; } = null!;
    public decimal CriptoAmount { get; set; }
    public decimal Money { get; set; }
    public string Action { get; set; } = null!;
    public DateTime Datetime { get; set; }
}

