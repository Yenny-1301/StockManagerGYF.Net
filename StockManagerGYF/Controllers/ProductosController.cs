using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagerGYF.Data.Repositories;
using StockManagerGYF.DTOs;
using StockManagerGYF.Model;

namespace StockManagerGYF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> obtenerProductos()
        {
            return Ok(await _productoRepository.obtenerProductos());
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> obtenerDetalleProducto(int id)
        {
            return Ok(await _productoRepository.obtenerDetalleProducto(id));
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> agregarProducto([FromBody] Producto prod)
        {
            if (prod == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var creado = await _productoRepository.insertarProducto(prod);

            return Created("created", creado);
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> actualizarProducto([FromBody] Producto prod)
        {
            if (prod == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productoRepository.actualizarProducto(prod);

            return NoContent();
        }

        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> eliminarProducto(int id)
        {
            await _productoRepository.eliminarProducto(new Producto { id = id });

            return NoContent();
        }

        [HttpGet("presupuesto/{monto}")]
        //[Authorize]
        public async Task<IActionResult> obtenerProductosPorMonto(int monto)
        {
            if (monto < 1 || monto > 1000000)
            {
                return BadRequest("El monto debe estar comprendido entre 1 y 1.000.000.");
            }

            var productos = await _productoRepository.obtenerProductos();
            Producto resultadoProductoA = null;
            Producto resultadoProductoB = null;
            int maxCombinacion = 0;

            var productosCategoriaA = productos
                .Where(p => p.categoria == "PRODUNO")
                .OrderBy(p => p.precio)
                .ToList();

            var productosCategoriaB = productos
                .Where(p => p.categoria == "PRODDOS")
                .OrderBy(p => p.precio)
                .ToList();

            if (!productosCategoriaA.Any() || !productosCategoriaB.Any())
            {
                return NotFound("No hay suficientes productos en ambas categorías para cumplir con la solicitud.");
            }

            int i = 0, j = productosCategoriaB.Count - 1;
            while (i < productosCategoriaA.Count && j >= 0)
            {
                int combinacionActual = productosCategoriaA[i].precio + productosCategoriaB[j].precio;

                if (combinacionActual <= monto)
                {
                    if (combinacionActual > maxCombinacion)
                    {
                        maxCombinacion = combinacionActual;
                        resultadoProductoA = productosCategoriaA[i];
                        resultadoProductoB = productosCategoriaB[j];
                    }
                    i++;
                }
                else
                {
                    j--; 
                }
            }

            if (resultadoProductoA == null || resultadoProductoB == null)
            {
                return NotFound("No se encontraron productos que cumplan con el presupuesto especificado.");
            }

            var respuesta = new ProductosPorCategoriaDTO
            {
                CategoriaA = resultadoProductoA,
                CategoriaB = resultadoProductoB
            };

            return Ok(respuesta);
        }

    }
}
