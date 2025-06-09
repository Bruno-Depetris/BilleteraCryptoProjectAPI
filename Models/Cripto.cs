using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Cripto {
    public string CriptoCode { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<HistorialPrecio> HistorialPrecios { get; set; } = new List<HistorialPrecio>();

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Operacione> Operaciones { get; set; } = new List<Operacione>();
}
