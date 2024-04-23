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
        public ProductoController(ILogger<ProductoController> logger, ApplicationDbContext db) //injectamos el loger
        {

            _logger = logger;
            _db = db;

        }

        //Get todos
        [HttpGet] //verbo http
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductoDto>> GetProductos()
        {
            _logger.LogInformation("todas las villas se estan opteniendo"); //mensaje en consola
            //return Ok(ProductoStore.productoList); //retornar los datos que esten en esa lista esto vieja forma desde array clase productStore
            return Ok(_db.Productos.ToList());//select de la tabla 
        }

        //Get one
        [HttpGet("id", Name = "GetProducto")] // ("id:int") verbo http indicamos el id porque sino da error por ser igual al de arriba
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(404)]
        public ActionResult<ProductoDto> GetProducto(int id)
        {
            //validar para retornar status
            if (id == 0)
            {
                return BadRequest(); //retorna estado 400
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            var producto = _db.Productos.FirstOrDefault(x => x.Id == id); //1 registro en base al id
            if (producto == null)
            {
                return NotFound(); //no encontro registro 404
            }

            return Ok(producto); //traer de listado por medio de lamda el id  y lo comparamos por el que pasamos
        }

        //create
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductoDto> CreateProducto([FromBody] ProductoDto producto) //dto datos que psasamos a travez del post
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if(ProductoStore.productoList.FirstOrDefault(x=>x.Nombre.ToLower() == producto.Nombre.ToLower()) != null){
            //    ModelState.AddModelError("El nombre ya existe", "El nombre del producto ya existe");
            //    return BadRequest(ModelState); //retornar un state custom
            //}
            if (_db.Productos.FirstOrDefault(x => x.Nombre.ToLower() == producto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("El nombre ya existe", "El nombre del producto ya existe");
                return BadRequest(ModelState); //retornar un state custom
            }

            if (producto == null)
            {
                return BadRequest(); //retorna 400
            } 

            if(producto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); //retorna 500
            }

            //usada
            Producto modelo = new()
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Detalle = producto.Detalle,
                Cantidad = producto.Cantidad,
                Tamaño = producto.Tamaño,
                ImageUrl = producto.ImageUrl
            };

            _db.Productos.Add(modelo); //methodo de entity framework add
            _db.SaveChanges(); //reflejar en la db

            //forma vieja
            //producto.Id = ProductoStore.productoList.OrderByDescending(x=>x.Id).FirstOrDefault().Id +1; //saca el ultimo id de la lista y le suma 1
            //ProductoStore.productoList.Add(producto); //agregar producto a la lista

            return CreatedAtRoute("GetProducto", new {id = producto.Id}, producto); // ruta, id, modelo(todo el producto)
        }

        //delete
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProducto(int id) //LA INTEFAZ NO UTILIZA EL MODELO retorna no content
        {
            if (id==0)
            {
                return BadRequest();
            }
            var producto = _db.Productos.FirstOrDefault(x => x.Id == id); //cambio
            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            //ProductoStore.productoList.Remove(producto);
            _db.Productos.Remove(producto);
            _db.SaveChanges();

            return NoContent();
        }

        //update put mas utilizado
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateProducto(int id, [FromBody] ProductoDto producto)
        {
            if(producto == null || id != producto.Id)
            {
                return BadRequest();
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            //producto.Nombre = producto.Nombre;
            //producto.Precio = producto.Precio;
            //producto.Cantidad = producto.Cantidad;

            var modelo = _db.Productos.FirstOrDefault(x => x.Id == id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.Nombre = producto.Nombre;
            modelo.Precio = producto.Precio;
            modelo.Detalle = producto.Detalle;
            modelo.Cantidad = producto.Cantidad;
            modelo.Tamaño = producto.Tamaño;
            modelo.ImageUrl = producto.ImageUrl;

            _db.Productos.Update(modelo); //methodo de entity framework Update
            _db.SaveChanges(); //reflejar en la db

            return NoContent();

        }

        //update patch
        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialProducto(int id, JsonPatchDocument<ProductoDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var producto = ProductoStore.productoList.FirstOrDefault(x => x.Id == id);
            var producto = _db.Productos.AsNoTracking().FirstOrDefault(x=> x.Id == id); //se agrego asnotracking

            ProductoDto productoDto = new()
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Detalle = producto.Detalle,
                Cantidad = producto.Cantidad,
                Tamaño = producto.Tamaño,
                ImageUrl = producto.ImageUrl
            };
            
            if(producto == null) return BadRequest();

            //patchDto.ApplyTo(producto, ModelState); //methodo propio de patch
            patchDto.ApplyTo(productoDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Producto modelo = new()
            {
                Id = productoDto.Id,
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio,
                Detalle = productoDto.Detalle,
                Cantidad = productoDto.Cantidad,
                Tamaño = productoDto.Tamaño,
                ImageUrl = productoDto.ImageUrl
            };

            _db.Productos.Update(modelo);
            _db.SaveChanges();
            return NoContent();

        }

    }
}
