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
public class EventoController : ControllerBase
{
    private readonly DataContext _context;

    public EventoController(DataContext dataContext)
    {
        _context = dataContext;
    }

[HttpGet]
public ActionResult<IEnumerable<Evento>> GetEventos()
{
    var eventos = _context.Eventos.ToList();
    return Ok(eventos);
}

[HttpGet("asisten")]
public ActionResult<IEnumerable<Evento>> GetEventosWithUsuarios()
{
    // Incluir la lista de usuarios asociados a cada evento
    var eventos = _context.Eventos.Include(e => e.usuarios).ToList();
    return Ok(eventos);
}


    [HttpGet("{id}")]
    public  ActionResult<Evento> GetEvento(int id)
    {
        var evento =  _context.Eventos.Find(id);
        if (evento == null)
        {
            return NotFound();
        }
        return Ok(evento);
    }

   [HttpPost]
public ActionResult<Evento> CreateEvento(Evento evento)
{
    // Asegúrate de que la lista de usuarios esté vacía antes de agregar el evento
    evento.usuarios = null;

    _context.Eventos.Add(evento);
    _context.SaveChanges();

    return CreatedAtAction(nameof(GetEvento), new { id = evento.id }, evento);
}



 [HttpPost("{idEvento}/asistir")]
    public IActionResult AsistirEvento(int idEvento, [FromBody] int idUsuario)
    {
        var evento = _context.Eventos.Include(e => e.usuarios).FirstOrDefault(e => e.id == idEvento);
        if (evento == null)
        {
            return NotFound("Evento no encontrado");
        }

        var usuario = _context.Usuarios.Find(idUsuario);
        if (usuario == null)
        {
            return NotFound("Usuario no encontrado");
        }

        if (!evento.asiste)
        {
            return BadRequest("El evento no permite asistencias");
        }

        if (evento.usuarios == null)
        {
            evento.usuarios = new List<Usuario>();
        }

        if (evento.usuarios.Any(u => u.id == idUsuario))
        {
            return BadRequest("El usuario ya está asistiendo a este evento");
        }

        evento.usuarios.Add(usuario);
        _context.SaveChanges();

        return Ok($"Usuario {usuario.nombreUsuario} ha sido añadido al evento {evento.nombre}");
    }

    [HttpPut("{id}")]
    public IActionResult  UpdateEvento(int id, Evento evento)
    {
        if (id != evento.id)
        {
            return BadRequest();
        }

        _context.Entry(evento).State = EntityState.Modified;
         _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvento(int id)
    {
        var evento =  _context.Eventos.Find(id);
        if (evento == null)
        {
            return NotFound();
        }

        _context.Eventos.Remove(evento);
         _context.SaveChanges();

        return NoContent();
    }
}
}