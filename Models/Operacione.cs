using System;
using System.Collections.Generic;

namespace BilleteraCryptoProjectAPI.Models;

public partial class Operacione {
    public int OperacionId { get; set; }

    public int ClienteId { get; set; }

    public string CriptoCode { get; set; } = null!;

    public decimal CriptoAmount { get; set; }

    public decimal Money { get; set; }

    public string Action { get; set; } = null!; // "purchase" o "sale"

    public DateTime Datetime { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Cripto CriptoCodeNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}


