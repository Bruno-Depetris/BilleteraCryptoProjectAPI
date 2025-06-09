public class OperacionReadDTO
{
    public int OperacionID { get; set; }
    public int CuentaID { get; set; }
    public string CriptoCode { get; set; } = null!;
    public decimal Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public int AccionID { get; set; }
    public decimal MontoARS { get; set; }

    public CuentaReadDTO? Cuenta { get; set; }
    public CriptoReadDTO? Cripto { get; set; }
    public AccionReadDTO? Accion { get; set; }
}