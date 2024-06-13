using Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Classes
{
    [ApiController]
    [Route("[Controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly DataContext _context;

        public ClienteController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAllClientes()
        {
            var clientes = _context.Clientes.ToList();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetClienteById(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

       [HttpPost]
public ActionResult<Cliente> AddCliente(Cliente cliente)
{
    var existingCliente = _context.Clientes.FirstOrDefault(c => c.email == cliente.email);
    if (existingCliente != null)
    {
        return Conflict("Ya existe un cliente con el mismo correo electrónico.");
    }

    _context.Clientes.Add(cliente);
    _context.SaveChanges();
    return CreatedAtAction(nameof(GetClienteById), new { id = cliente.id }, cliente);
}


        [HttpPut("{id}")]
        public IActionResult UpdateCliente(int id, Cliente clienteActualizado)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.nombre = clienteActualizado.nombre;
            // Actualiza otros atributos según sea necesario

            _context.Clientes.Update(cliente);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCliente(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("email/{email}")]
        public ActionResult<Cliente> GetClienteByEmail(string email)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.email == email);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpGet("login")]
        public ActionResult<Cliente> GetClienteByEmailAndPassword([FromQuery] string email, [FromQuery] string password)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.email == email && c.password == password);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }


        
    }
}
