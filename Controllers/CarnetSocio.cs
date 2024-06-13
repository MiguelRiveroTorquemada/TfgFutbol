using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Stripe.Checkout;
using Stripe;

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
                        StripeConfiguration.ApiKey = "sk_test_51OKO1bLkVoGrpMmaMHXpcOmlq3e9mG4H8sMpUIHQGLcNYISgq9EZohU3VkvspeGmyDDJpEI85QJBYAQ7e05sBoz2006yRX5cR8";

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

       /* [HttpPost]
        public ActionResult<CarnetSocio> AddCarnetSocio(CarnetSocio carnet)
        {
            _context.CarnetSocios.Add(carnet);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCarnetSocioById), new { id = carnet.id }, carnet);
        }
*/
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

    


        [HttpPost("CreateStripeSession")]
public async Task<ActionResult<string>> CreateStripeSession([FromBody] CarnetSocio carnetSocio)
{
    try
    {
        
        // Guarda el pago en la base de datos para obtener el ID del pago
        _context.CarnetSocios.Add(carnetSocio);
        _context.SaveChanges();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Producto",
                        },
                        UnitAmount = (long)(carnetSocio.precio * 100),
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "http://localhost:8080/#/",
            CancelUrl = "https://tu-sitio.com/cancel",
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        // Asigna el ID de la sesión de Stripe al pago
        carnetSocio.StripeSessionId = session.Id;

        // Actualiza la entidad Pago en la base de datos con el StripeSessionId
        _context.CarnetSocios.Update(carnetSocio);
        _context.SaveChanges();

      
        return Ok(new { sessionId = session.Id });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al crear la sesión de Stripe: {ex.Message}");
    }
}
    }
    
}