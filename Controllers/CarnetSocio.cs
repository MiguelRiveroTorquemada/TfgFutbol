using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Classes
{
     [ApiController]
    [Route("[Controller]")]
    public class CarnetSocioController : ControllerBase
    {
        private readonly DataContext _context;

        public CarnetSocioController(DataContext dataContext)
        {
            _context = dataContext;
        }


       [HttpGet]
public ActionResult<IEnumerable<CarnetSocio>> GetAllCarnetSocios()
{
    var carnets = _context.CarnetSocios
        .Include(c => c.cliente) // Incluir los datos del cliente asociado a cada carnet
        .ToList();
    
    return Ok(carnets);
}
[HttpGet("ByEmail")]
public ActionResult<CarnetSocio> GetCarnetSocioByEmail(string email)
{
    var carnet = _context.CarnetSocios.FirstOrDefault(c => c.cliente.email == email);
    if (carnet == null)
    {
        return NotFound();
    }
    return Ok(carnet);
}

        [HttpGet("{id}")]
        public ActionResult<CarnetSocio> GetCarnetSocioById(int id)
        {
            var carnet = _context.CarnetSocios.FirstOrDefault(c => c.id == id);
            if (carnet == null)
            {
                return NotFound();
            }
            return Ok(carnet);
        }

        [HttpPost]
        public ActionResult<CarnetSocio> AddCarnetSocio(CarnetSocio carnet)
        {
            _context.CarnetSocios.Add(carnet);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCarnetSocioById), new { id = carnet.id }, carnet);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCarnetSocio(int id, CarnetSocio carnetActualizado)
        {
            var carnet = _context.CarnetSocios.FirstOrDefault(c => c.id == id);
            if (carnet == null)
            {
                return NotFound();
            }

            carnet.cliente = carnetActualizado.cliente;
            // Actualiza otros atributos según sea necesario

            _context.CarnetSocios.Update(carnet);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCarnetSocio(int id)
        {
            var carnet = _context.CarnetSocios.FirstOrDefault(c => c.id == id);
            if (carnet == null)
            {
                return NotFound();
            }

            _context.CarnetSocios.Remove(carnet);
            _context.SaveChanges();

            return NoContent();
        }
    }
}