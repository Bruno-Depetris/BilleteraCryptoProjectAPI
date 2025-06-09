using System.ComponentModel.DataAnnotations;

public class ClienteCreateDTO
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}