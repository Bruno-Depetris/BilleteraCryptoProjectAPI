using System.ComponentModel.DataAnnotations;

public class MovimientoCreateDTO
{
    [Required]
    public int OperacionID { get; set; }

    [Required]
    public string CriptoCode { get; set; } = null!;

    [Required]
    public decimal CantidadCripto { get; set; }

    [Required]
    public decimal EstadoBilletera { get; set; }

    [Required]
    public DateTime Fecha { get; set; }
}