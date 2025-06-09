public class MovimientoReadDTO
{
    public int MovimientoID { get; set; }
    public int OperacionID { get; set; }
    public string CriptoCode { get; set; } = null!;
    public decimal CantidadCripto { get; set; }
    public decimal EstadoBilletera { get; set; }
    public DateTime Fecha { get; set; }

    public OperacionReadDTO? Operacion { get; set; }
    public CriptoReadDTO? Cripto { get; set; }
}