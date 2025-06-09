using System.ComponentModel.DataAnnotations;

public class OperacionCreateDTO
{
    [Required]
    public int CuentaID { get; set; }

    [Required]
    public string CriptoCode { get; set; } = null!;

    [Required]
    public decimal Cantidad { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public int AccionID { get; set; }

    [Required]
    public decimal MontoARS { get; set; }
}