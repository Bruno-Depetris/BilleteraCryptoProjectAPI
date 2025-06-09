public class HistorialPrecioReadDTO
{
    public int HistorialID { get; set; }
    public string CriptoCode { get; set; } = null!;
    public decimal Precio { get; set; }
    public DateTime Fecha { get; set; }
    public string? Fuente { get; set; }

    public CriptoReadDTO? Cripto { get; set; }
}