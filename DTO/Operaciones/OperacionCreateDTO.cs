using System.ComponentModel.DataAnnotations;

public class OperacionCreateDTO
{
    [Required]
    public int ClienteID { get; set; }

    [Required]
    [StringLength(20)]
    public string CriptoCode { get; set; } = null!;

    [Required]
    public decimal CriptoAmount { get; set; }

    [Required]
    [StringLength(20)]
    public string Action { get; set; } = null!; // "purchase" o "sale"

    [Required]
    public DateTime Datetime { get; set; }
}

