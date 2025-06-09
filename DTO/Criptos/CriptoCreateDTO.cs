using System.ComponentModel.DataAnnotations;

public class CriptoCreateDTO
{
    [Required]
    [StringLength(20)]
    public string CriptoCode { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;
}