using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentaProducto_API.Modelos
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //id incremental automaticamente
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public string Detalle { get; set; }
        public int Cantidad { get; set; }
        public string Tamaño { get; set; } 
        public string ImageUrl { get; set;}
        public DateTime FechaCreacion { get; set;}
        public DateTime FechaAcualizacion { get; set;}
    }
}


