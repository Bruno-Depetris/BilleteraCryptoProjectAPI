using System.ComponentModel.DataAnnotations;

public class CuentaUpdateDTO
{
    [Required]
    public int CuentaID { get; set; }

    [Required]
    public int ClienteID { get; set; }

    [Required]
    public int EstadoID { get; set; }
}