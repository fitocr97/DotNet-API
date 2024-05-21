using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Xml.Linq;
using VentaProducto_API.Datos;
using VentaProducto_API.Modelos;
using VentaProducto_API.Modelos.Dto;

namespace VentaProducto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger; //variable privadas _variable "_"
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductoController(ILogger<ProductoController> logger, ApplicationDbContext db, IMapper  mapper) //injectamos el loger
        {

            _logger = logger;
            _db = db;
            _mapper = mapper;

        }

        //Get todos
        [HttpGet] //verbo http
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            _logger.LogInformation("todas las villas se estan opteniendo"); //mensaje en consola
            //return Ok(ProductoStore.productoList); //retornar los datos que esten en esa lista esto vieja forma desde array clase productStore

            //usando mapper
            IEnumerable<Producto> productoList = await _db.Productos.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductoDto>>(productoList));//select de la tabla 

            //sin mapper
            //return Ok(await _db.Productos.ToListAsync());//select de la tabla 
        }

        //Get one
        [HttpGet("id", Name = "GetProducto")] // ("id:int") verbo http indicamos el id porque sino da error por ser igual al de arriba
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(404)]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            //validar para retornar status
            if (id == 0)
            {
                return BadRequest(); //retorna estado 400
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            var producto = await _db.Productos.FirstOrDefaultAsync(x => x.Id == id); //1 registro en base al id
            if (producto == null)
            {
                return NotFound(); //no encontro registro 404
            }
            //normal
            //return Ok(producto); //traer de listado por medio de lamda el id  y lo comparamos por el que pasamos

            //automaper
            return Ok(_mapper.Map<ProductoDto>(producto));
        }

        //create
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoDto>> CreateProducto([FromBody] ProductoCreateDto producto) //dto datos que psasamos a travez del post
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if(ProductoStore.productoList.FirstOrDefault(x=>x.Nombre.ToLower() == producto.Nombre.ToLower()) != null){
            //    ModelState.AddModelError("El nombre ya existe", "El nombre del producto ya existe");
            //    return BadRequest(ModelState); //retornar un state custom
            //}
            if (await _db.Productos.FirstOrDefaultAsync(x => x.Nombre.ToLower() == producto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("El nombre ya existe", "El nombre del producto ya existe");
                return BadRequest(ModelState); //retornar un state custom
            }

            if (producto == null)
            {
                return BadRequest(producto); //retorna 400
            }

            /* se elimian esta validacion al usar ProductoCreateDto
            if(producto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); //retorna 500
            }*/

            //usada antes del authomaper
            /*Producto modelo = new()
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Detalle = producto.Detalle,
                Cantidad = producto.Cantidad,
                Tamaño = producto.Tamaño,
                ImageUrl = producto.ImageUrl
            };*/

            //usando mapper para hacer mas limpio no usar lo de arriba
            Producto modelo = _mapper.Map<Producto>(producto);

            await _db.Productos.AddAsync(modelo); //methodo de entity framework add
            await _db.SaveChangesAsync(); //reflejar en la db

            //forma vieja
            //producto.Id = ProductoStore.productoList.OrderByDescending(x=>x.Id).FirstOrDefault().Id +1; //saca el ultimo id de la lista y le suma 1
            //ProductoStore.productoList.Add(producto); //agregar producto a la lista

            return CreatedAtRoute("GetProducto", new {id = modelo.Id}, modelo); // ruta, id, modelo(todo el producto)
        }

        //delete no ocupa mapeo
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProducto(int id) //LA INTEFAZ NO UTILIZA EL MODELO retorna no content
        {
            if (id==0)
            {
                return BadRequest();
            }
            var producto = await _db.Productos.FirstOrDefaultAsync(x => x.Id == id); //cambio
            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            //ProductoStore.productoList.Remove(producto);
            _db.Productos.Remove(producto);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        //update put mas utilizado
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateProducto(int id, [FromBody] ProductoUpdateDto producto)
        {
            if(producto == null || id != producto.Id)
            {
                return BadRequest();
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            //producto.Nombre = producto.Nombre;
            //producto.Precio = producto.Precio;
            //producto.Cantidad = producto.Cantidad;


            //antes del mapper
            /*
            var modelo = await _db.Productos.FirstOrDefaultAsync(x => x.Id == id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.Nombre = producto.Nombre;
            modelo.Precio = producto.Precio;
            modelo.Detalle = producto.Detalle;
            modelo.Cantidad = producto.Cantidad;
            modelo.Tamaño = producto.Tamaño;
            modelo.ImageUrl = producto.ImageUrl;*/

            //usando mapper
            Producto modelo = _mapper.Map<Producto>(producto);

            _db.Productos.Update(modelo); //methodo de entity framework Update
            await _db.SaveChangesAsync(); //reflejar en la db

            return NoContent();

        }

        //update patch
        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialProducto(int id, JsonPatchDocument<ProductoUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            var producto = await _db.Productos.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == id); //se agrego asnotracking

            //antes del mapper
            /*
            ProductoUpdateDto productoDto = new()
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Detalle = producto.Detalle,
                Cantidad = producto.Cantidad,
                Tamaño = producto.Tamaño,
                ImageUrl = producto.ImageUrl
            };*/

            //despues del mapper
            ProductoUpdateDto productoDto = _mapper.Map<ProductoUpdateDto>(producto);


            if (producto == null) return BadRequest();

            //patchDto.ApplyTo(producto, ModelState); //methodo propio de patch
            patchDto.ApplyTo(productoDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //antes del mapper igual que arriba
            /*
            Producto modelo = new()
            {
                Id = productoDto.Id,
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio,
                Detalle = productoDto.Detalle,
                Cantidad = productoDto.Cantidad,
                Tamaño = productoDto.Tamaño,
                ImageUrl = productoDto.ImageUrl
            };*/

            //despues del mapper
            Producto modelo = _mapper.Map<Producto>(productoDto);

            _db.Productos.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();

        }

    }
}
