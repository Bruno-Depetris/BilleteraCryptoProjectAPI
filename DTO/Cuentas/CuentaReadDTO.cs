public class CuentaReadDTO
{
    public int CuentaID { get; set; }
    public int ClienteID { get; set; }
    public int EstadoID { get; set; }

    public ClienteReadDTO? Cliente { get; set; }
    public EstadoReadDTO? Estado { get; set; }
}