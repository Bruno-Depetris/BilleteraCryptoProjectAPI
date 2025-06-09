using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Operacione {
    public int OperacionId { get; set; }

    public int CuentaId { get; set; }

    public string CriptoCode { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    public int AccionId { get; set; }

    public decimal MontoArs { get; set; }

    public virtual Accione Accion { get; set; } = null!;

    public virtual Cripto CriptoCodeNavigation { get; set; } = null!;

    public virtual Cuenta Cuenta { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
