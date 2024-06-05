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
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly DataContext _context;

    public UsuarioController(DataContext dataContext)
    {
        _context = dataContext;
    }

 [HttpGet]
    public ActionResult<IEnumerable<Usuario>> GetUsuarios()
    {
        var usuarios = _context.Usuarios.ToList();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public ActionResult<Usuario> GetUsuario(int id)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.id == id);
        if (usuario == null)
        {
            return NotFound();
        }
        return Ok(usuario);
    }

    [HttpPost]
    public ActionResult<Usuario> CreateUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.id }, usuario);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUsuario(int id, Usuario usuario)
    {
        if (id != usuario.id)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        var usuario = _context.Usuarios.Find(id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return NoContent();
    }
}
}