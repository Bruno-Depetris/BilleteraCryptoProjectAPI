using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Cuenta {
    public int CuentaId { get; set; }

    public int ClienteId { get; set; }

    public int EstadoId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<Operacione> Operaciones { get; set; } = new List<Operacione>();
}
