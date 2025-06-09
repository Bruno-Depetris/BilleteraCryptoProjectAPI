using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Cliente {
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Cuenta> Cuenta { get; set; } = new List<Cuenta>();
}
