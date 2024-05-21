using AutoMapper;
using VentaProducto_API.Modelos;
using VentaProducto_API.Modelos.Dto;

namespace VentaProducto_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Producto, ProductoDto>(); //indicar fuente y destino
            CreateMap<ProductoDto, Producto>();

            CreateMap<Producto, ProductoCreateDto>().ReverseMap();
            CreateMap<Producto, ProductoUpdateDto>().ReverseMap();

        }
    }
}
