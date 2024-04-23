using VentaProducto_API.Modelos.Dto;

namespace VentaProducto_API.Datos
{
    public class ProductoStore
    {
        public static List<ProductoDto> productoList = new List<ProductoDto>
        {
            new ProductoDto{Id=1, Nombre="Colibri", Precio=200,Cantidad=3 }, //simular datos que devuelve la bd
            new ProductoDto{Id=2, Nombre="Corazon",Precio=300,Cantidad=4 }, //simular datos que devuelve la bd
            new ProductoDto{Id=3, Nombre="Shell",Precio=400,Cantidad=7 }, //simular datos que devuelve la bd
            new ProductoDto{Id=4, Nombre="Rana",Precio=500,Cantidad=10 } //simular datos que devuelve la bd
        };
    }
}
