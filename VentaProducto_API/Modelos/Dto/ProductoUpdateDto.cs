using System.ComponentModel.DataAnnotations;

namespace VentaProducto_API.Modelos.Dto
{
    public class ProductoUpdateDto //evitar usar los modelos de la bd directamente
    {
        [Required]
        public int Id { get; set; } //atributos que se expondran
        //data anotations
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; } //atributos que se expondran
        [Required]
        public int Precio { get; set; } //atributos que se expondran
        [Required]
        public int Cantidad { get; set; } //atributos que se expondrans
        [Required]
        public string Detalle { get; set; }
        [Required]
        public string Tamaño { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
