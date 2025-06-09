using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Movimiento {
    public int MovimientoId { get; set; }

    public int OperacionId { get; set; }

    public string CriptoCode { get; set; } = null!;

    public decimal CantidadCripto { get; set; }

    public decimal EstadoBilletera { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Cripto CriptoCodeNavigation { get; set; } = null!;

    public virtual Operacione Operacion { get; set; } = null!;
}
