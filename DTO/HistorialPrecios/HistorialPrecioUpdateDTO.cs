using System.ComponentModel.DataAnnotations;

namespace BilleteraCryptoProjectAPI.DTO.HistorialPrecios {
    public class HistorialPrecioUpdateDTO {

        [Key]
        public int HistorialPrecioID { get; set; }
        [Required]
        public string CriptoCode { get; set; } = null!;

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string? Fuente { get; set; }

    }
}
