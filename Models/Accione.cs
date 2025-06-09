using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Accione {
    public int AccionId { get; set; }

    public string Accion { get; set; } = null!;

    public virtual ICollection<Operacione> Operaciones { get; set; } = new List<Operacione>();
}
