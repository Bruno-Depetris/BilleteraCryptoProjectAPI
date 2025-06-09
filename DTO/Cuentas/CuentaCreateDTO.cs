using System.ComponentModel.DataAnnotations;

public class CuentaCreateDTO
{
    [Required]
    public int ClienteID { get; set; }

    [Required]
    public int EstadoID { get; set; }
}