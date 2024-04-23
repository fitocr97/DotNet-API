using System.ComponentModel.DataAnnotations;

namespace VentaProducto_API.Modelos.Dto
{
    public class ProductoDto //evitar usar los modelos de la bd directamente
    {
        [Key]
        public int Id { get; set; } //atributos que se expondran
        //data anotations
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; } //atributos que se expondran
        public int Precio { get; set; } //atributos que se expondran
        public int Cantidad { get; set; } //atributos que se expondrans
        public string Detalle { get; set; }
        public string Tamaño { get; set; }
        public string ImageUrl { get; set; }
    }
}
