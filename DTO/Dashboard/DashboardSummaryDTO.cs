public class DashboardSummaryDTO
{
    public int ClienteID { get; set; }
    public decimal TotalMoney { get; set; }
    public List<DashboardItemDTO> Items { get; set; } = new();
}


