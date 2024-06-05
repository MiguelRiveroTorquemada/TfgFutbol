using Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Classes
{
     [ApiController]
    [Route("[Controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductoController(DataContext dataContext)
        {
            _context = dataContext;
        }

             [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAllProductos()
        {
            var productos = _context.Productos.ToList();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> GetProductoById(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Producto> AddProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProductoById), new { id = producto.id }, producto);
        }

       [HttpPut("{id}")]
public IActionResult UpdateProducto(int id, Producto productoActualizado)
{
    var producto = _context.Productos.FirstOrDefault(p => p.id == id);
    if (producto == null)
    {
        return NotFound();
    }

    producto.nombreProducto = productoActualizado.nombreProducto;
    producto.precio = productoActualizado.precio;

    _context.Productos.Update(producto);
    _context.SaveChanges();

    return NoContent();
}

[HttpDelete("{id}")]
public IActionResult DeleteProducto(int id)
{
    var producto = _context.Productos.FirstOrDefault(p => p.id == id);
    if (producto == null)
    {
        return NotFound();
    }

    _context.Productos.Remove(producto);
    _context.SaveChanges();

    return NoContent();
}

    }

    }