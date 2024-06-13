using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Stripe;
using Stripe.Checkout;


namespace Classes
{
    [ApiController]
    [Route("[Controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly DataContext _context;

        public CarritoController(DataContext dataContext)
        {
            _context = dataContext;
            StripeConfiguration.ApiKey = "sk_test_51OKO1bLkVoGrpMmaMHXpcOmlq3e9mG4H8sMpUIHQGLcNYISgq9EZohU3VkvspeGmyDDJpEI85QJBYAQ7e05sBoz2006yRX5cR8";

        }

        [HttpGet]
        public ActionResult<IEnumerable<Carrito>> GetAllCarritos()
        {
            var carritos = _context.Carritos
                                   .Include(c => c.clientes) // Incluir los clientes
                                   .ToList();

            // Carga explícita de los productos para cada carrito
            foreach (var carrito in carritos)
            {
                _context.Entry(carrito)
                        .Collection(c => c.items)
                        .Load();
            }

            return Ok(carritos);
        }

        [HttpGet("{id}")]
        public ActionResult<Carrito> GetCarritoById(int id)
        {
            var carrito = _context.Carritos
                                  .Include(c => c.clientes) // Incluir los clientes
                                  .FirstOrDefault(c => c.id == id);
            if (carrito == null)
            {
                return NotFound();
            }
            return Ok(carrito);
        }

      /*  [HttpPost]
        public ActionResult<Carrito> AddCarrito(int? carritoId, List<int> productoIds, int? carnetSocioId, string email, string password)
        {
            try
            {
                // Verificar si email es proporcionado
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("El email del cliente es obligatorio.");
                }

                // Verificar si contraseña es proporcionada
                if (string.IsNullOrEmpty(password))
                {
                    return BadRequest("La contraseña del cliente es obligatoria.");
                }

                // Crea una nueva instancia de carrito si no se proporciona un id de carrito
                Carrito carrito = null;
                if (carritoId.HasValue)
                {
                    carrito = _context.Carritos.FirstOrDefault(c => c.id == carritoId);
                    if (carrito == null)
                    {
                        return NotFound($"No se encontró un carrito con el ID {carritoId}.");
                    }
                }
                else
                {
                    carrito = new Carrito
                    {
                        items = new List<Producto>(), // Inicializar la lista de productos
                        carnetSocios = new List<CarnetSocio>(), // Inicializar la lista de carnets socio
                        clientes = new List<Cliente>() // Inicializar la lista de clientes
                    };
                }

                // Si se proporcionan identificadores de productos, busca y agrega los productos al carrito
                if (productoIds != null && productoIds.Any())
                {
                    foreach (var productoId in productoIds)
                    {
                        var producto = _context.Productos.FirstOrDefault(p => p.id == productoId);
                        if (producto == null)
                        {
                            return NotFound($"No se encontró un producto con el ID {productoId}.");
                        }

                        carrito.items.Add(producto);
                        carrito.total += producto.precio;
                    }
                }

                // Si se proporciona un id de carnet de socio, busca y agrega el carnet de socio al carrito
                if (carnetSocioId.HasValue)
                {
                    var carnetSocio = _context.CarnetSocios.FirstOrDefault(cs => cs.id == carnetSocioId);
                    if (carnetSocio == null)
                    {
                        return NotFound($"No se encontró un carnet de socio con el ID {carnetSocioId}.");
                    }

                    carrito.carnetSocios.Add(carnetSocio);
                    carrito.total += carnetSocio.precio;
                }

                // Busca el cliente por email
                var cliente = _context.Clientes.FirstOrDefault(c => c.email == email);
                if (cliente == null)
                {
                    // Si el cliente no existe, crea uno nuevo con la contraseña proporcionada
                    cliente = new Cliente
                    {
                        email = email,
                        password = password
                        // Otros atributos del cliente, como nombre, apellidos, etc.
                    };
                    _context.Clientes.Add(cliente);
                }
                else
                {
                    // Si el cliente existe, verifica que la contraseña coincida
                    if (cliente.password != password)
                    {
                        return BadRequest("La contraseña proporcionada no es válida para este cliente.");
                    }
                }

                carrito.clientes.Add(cliente);

                // Si no se especificó ningún producto ni carnet de socio, retorna un mensaje de error
                if ((productoIds == null || !productoIds.Any()) && !carnetSocioId.HasValue)
                {
                    return BadRequest("Se debe proporcionar al menos un producto o un carnet de socio.");
                }

                // Si el carrito es nuevo, agrégalo al contexto de datos
                if (!carritoId.HasValue)
                {
                    _context.Carritos.Add(carrito);
                }

                // Guarda los cambios en el contexto de datos
                _context.SaveChanges();

                // Devuelve una respuesta con el carrito creado
                return CreatedAtAction(nameof(GetCarritoById), new { id = carrito.id }, carrito);
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el proceso
                return StatusCode(500, $"Error al agregar los productos, el carnet de socio y el cliente al carrito: {ex.Message}");
            }
        }

*/
        [HttpPut("{id}")]
        public IActionResult UpdateCarrito(int id, Carrito carritoActualizado)
        {
            var carrito = _context.Carritos.FirstOrDefault(c => c.id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            carrito.enviado = carritoActualizado.enviado; // Actualiza el campo enviado

            _context.Carritos.Update(carrito);
            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCarrito(int id)
        {
            var carrito = _context.Carritos.FirstOrDefault(c => c.id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            _context.Carritos.Remove(carrito);
            _context.SaveChanges();

            return NoContent();
        }
[HttpPost("CreateStripeSession")]
public ActionResult<string> CreateStripeSession(int? carritoId, List<int> productoIds, int? carnetSocioId, string email, string password)
{
    try
    {
        // Verificar si email es proporcionado
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("El email del cliente es obligatorio.");
        }

        // Verificar si contraseña es proporcionada
        if (string.IsNullOrEmpty(password))
        {
            return BadRequest("La contraseña del cliente es obligatoria.");
        }

        // Crea una nueva instancia de carrito si no se proporciona un id de carrito
        Carrito carrito = null;
        if (carritoId.HasValue)
        {
            carrito = _context.Carritos.FirstOrDefault(c => c.id == carritoId);
            if (carrito == null)
            {
                return NotFound($"No se encontró un carrito con el ID {carritoId}.");
            }
        }
        else
        {
            carrito = new Carrito
            {
                items = new List<Producto>(), // Inicializar la lista de productos
                carnetSocios = new List<CarnetSocio>(), // Inicializar la lista de carnets socio
                clientes = new List<Cliente>(), // Inicializar la lista de clientes
                StripeCustomerId = "", // Proporcionar un valor por defecto
                StripeSessionId = "" // Proporcionar un valor por defecto para evitar problemas de nulos
            };
        }

        // Si se proporcionan identificadores de productos, busca y agrega los productos al carrito
        if (productoIds != null && productoIds.Any())
        {
            foreach (var productoId in productoIds)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.id == productoId);
                if (producto == null)
                {
                    return NotFound($"No se encontró un producto con el ID {productoId}.");
                }

                carrito.items.Add(producto);
                carrito.total += producto.precio;
            }
        }

        // Si se proporciona un id de carnet de socio, busca y agrega el carnet de socio al carrito
        if (carnetSocioId.HasValue)
        {
            var carnetSocio = _context.CarnetSocios.FirstOrDefault(cs => cs.id == carnetSocioId);
            if (carnetSocio == null)
            {
                return NotFound($"No se encontró un carnet de socio con el ID {carnetSocioId}.");
            }

            carrito.carnetSocios.Add(carnetSocio);
            carrito.total += carnetSocio.precio;
        }

        // Busca el cliente por email
        var cliente = _context.Clientes.FirstOrDefault(c => c.email == email);
        if (cliente == null)
        {
            // Si el cliente no existe, crea uno nuevo con la contraseña proporcionada
            cliente = new Cliente
            {
                email = email,
                password = password
                // Otros atributos del cliente, como nombre, apellidos, etc.
            };
            _context.Clientes.Add(cliente);
        }
        else
        {
            // Si el cliente existe, verifica que la contraseña coincida
            if (cliente.password != password)
            {
                return BadRequest("La contraseña proporcionada no es válida para este cliente.");
            }
        }

        carrito.clientes.Add(cliente);

        // Si no se especificó ningún producto ni carnet de socio, retorna un mensaje de error
        if ((productoIds == null || !productoIds.Any()) && !carnetSocioId.HasValue)
        {
            return BadRequest("Se debe proporcionar al menos un producto o un carnet de socio.");
        }

        // Si el carrito es nuevo, agrégalo al contexto de datos
        if (!carritoId.HasValue)
        {
            _context.Carritos.Add(carrito);
        }

        // Guarda los cambios en el contexto de datos para obtener el ID del carrito
        _context.SaveChanges();

        // Crear la sesión de Stripe
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
                            Name = "Productos y Carnets",
                        },
                        UnitAmount = (long)(carrito.total * 100),
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "http://localhost:8080/#/",
            CancelUrl = "https://tu-sitio.com/cancel",
        };

        var service = new SessionService();
        var session = service.Create(options);

        // Asigna el ID de la sesión de Stripe al carrito
        carrito.StripeSessionId = session.Id;

        // Actualiza el carrito en la base de datos con el StripeSessionId
        _context.Carritos.Update(carrito);
        _context.SaveChanges();

        return Ok(new { sessionId = session.Id });
    }
    catch (DbUpdateException dbEx)
    {
        // Captura detalles específicos del error en la base de datos
        var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
        return StatusCode(500, $"Error al guardar en la base de datos: {innerExceptionMessage}");
    }
    catch (Exception ex)
    {
        // Maneja cualquier otra excepción que pueda ocurrir
        return StatusCode(500, $"Error al crear la sesión de Stripe: {ex.Message}");
    }
}


[HttpGet("ByClienteId/{clienteId}")]
public ActionResult<IEnumerable<Carrito>> GetCarritosByClienteId(int clienteId)
{
    var carritos = _context.Carritos
                           .Include(c => c.clientes) // Incluir los clientes
                           .Where(c => c.clientes.Any(cli => cli.id == clienteId))
                           .ToList();

    // Carga explícita de los productos para cada carrito
    foreach (var carrito in carritos)
    {
        _context.Entry(carrito)
                .Collection(c => c.items)
                .Load();
    }

    return Ok(carritos);
}

    }
}

