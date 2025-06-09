using System.ComponentModel.DataAnnotations;

public class EstadoUpdateDTO
{
    [Required]
    public int EstadoID { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = null!;
}