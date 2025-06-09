using System.ComponentModel.DataAnnotations;

public class HistorialPrecioCreateDTO
{
    [Required]
    public string CriptoCode { get; set; } = null!;

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    public string? Fuente { get; set; }
}