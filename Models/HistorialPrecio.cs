using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class HistorialPrecio {
    public int HistorialId { get; set; }

    public string CriptoCode { get; set; } = null!;

    public decimal Precio { get; set; }

    public DateTime Fecha { get; set; }

    public string? Fuente { get; set; }

    public virtual Cripto CriptoCodeNavigation { get; set; } = null!;
}
