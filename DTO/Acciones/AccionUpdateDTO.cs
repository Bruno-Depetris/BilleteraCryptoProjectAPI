using System.ComponentModel.DataAnnotations;

public class AccionUpdateDTO
{
    [Required]
    public int AccionID { get; set; }

    [Required]
    [StringLength(20)]
    public string Accion { get; set; } = null!;
}