using System.ComponentModel.DataAnnotations;

public class AccionCreateDTO
{
    [Required]
    [StringLength(20)]
    public string Accion { get; set; } = null!;
}