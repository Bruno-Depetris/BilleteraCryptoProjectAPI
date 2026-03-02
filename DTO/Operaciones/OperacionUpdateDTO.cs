using System.ComponentModel.DataAnnotations;

public class OperacionUpdateDTO
{
    public decimal? Money { get; set; }
    
    [StringLength(20)]
    public string? CriptoCode { get; set; }
    
    public decimal? CriptoAmount { get; set; }
    
    [StringLength(20)]
    public string? Action { get; set; }
    
    public DateTime? Datetime { get; set; }
}


