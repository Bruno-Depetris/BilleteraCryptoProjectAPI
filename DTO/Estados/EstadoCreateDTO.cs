using System.ComponentModel.DataAnnotations;

public class EstadoCreateDTO
{
    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = null!;
}