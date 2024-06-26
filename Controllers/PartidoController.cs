using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Classes;
using Data;
using System.Net;

namespace Classes
{
    [ApiController]
    [Route("[Controller]")]
    public class PartidosController : ControllerBase
    {
        private readonly DataContext _context;

        public PartidosController(DataContext context)
        {
            _context = context;
        }
[HttpGet]
public ActionResult<IEnumerable<Partido>> GetPartidos()
{
    var partidos = _context.Partidos.Include(p => p.jugadores).ToList();

    // Actualizar el ID de los jugadores con el mismo número de camiseta
    var jugadoresPorNumeroCamiseta = new Dictionary<int, int>();
    foreach(var partido in partidos)
    {
        foreach(var jugador in partido.jugadores)
        {
            if (!jugadoresPorNumeroCamiseta.ContainsKey(jugador.numeroCamiseta))
            {
                jugadoresPorNumeroCamiseta[jugador.numeroCamiseta] = jugador.id;
            }
            else
            {
                jugador.id = jugadoresPorNumeroCamiseta[jugador.numeroCamiseta];
            }
        }
    }

    return partidos;
}



        // Endpoint para obtener un partido por su ID
        [HttpGet("{id}")]
        public ActionResult<Partido> GetPartido(int id)
        {
            var partido = _context.Partidos.Find(id);

            if (partido == null)
            {
                return NotFound();
            }

            return partido;
        }

        // Endpoint para agregar un nuevo partido
        [HttpPost]
        public IActionResult PostPartido(Partido partido)
        {
            _context.Partidos.Add(partido);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPartido), new { id = partido.id }, partido);
        }

 /*[HttpPut("{id}")]
public IActionResult PutPartido(int id, [FromBody] Partido partido)
{
    var partidoToUpdate = _context.Partidos
                                  .Include(p => p.jugadores)
                                  .FirstOrDefault(p => p.id == id);

    if (partidoToUpdate == null)
    {
        return NotFound();
    }

    // Actualizar propiedades del partido
    partidoToUpdate.nombrePartido = partido.nombrePartido;
    partidoToUpdate.ganado = partido.ganado;
    partidoToUpdate.puntuacion = partido.puntuacion;
        partidoToUpdate.fecha = partido.fecha;
    partidoToUpdate.primerEquipo = partido.primerEquipo;

    partidoToUpdate.segundoEquipo = partido.segundoEquipo;
    // Actualizar otras propiedades del partido...

    // Actualizar jugadores
    foreach (var jugador in partido.jugadores)
    {
        var existingJugador = partidoToUpdate.jugadores.FirstOrDefault(j => j.numeroCamiseta == jugador.numeroCamiseta);
        if (existingJugador != null)
        {
            // Actualizar propiedades del jugador
            existingJugador.goles = jugador.goles;
            existingJugador.posicion = jugador.posicion;
            existingJugador.partidosJugados = jugador.partidosJugados;
            // Actualizar otras propiedades del jugador...
        }
        else
        {
            // Si el jugador no existe en la lista actual, agregarlo
            partidoToUpdate.jugadores.Add(jugador);
        }
    }

    _context.SaveChanges();

    return NoContent();
}
*/
[HttpPut("{id}")]
public IActionResult PutPartidoOnly(int id, [FromBody] Partido partido)
{
    var partidoToUpdate = _context.Partidos.FirstOrDefault(p => p.id == id);

    if (partidoToUpdate == null)
    {
        return NotFound();
    }

    // Actualizar propiedades del partido
    partidoToUpdate.nombrePartido = partido.nombrePartido;
    partidoToUpdate.ganado = partido.ganado;
    partidoToUpdate.puntuacion = partido.puntuacion;
    partidoToUpdate.fecha = partido.fecha;
    partidoToUpdate.primerEquipo = partido.primerEquipo;
    partidoToUpdate.segundoEquipo = partido.segundoEquipo;
    // Actualizar otras propiedades del partido...

    _context.SaveChanges();

    return NoContent();
}
[HttpPut("{id}/jugadores")]
public IActionResult PutJugadores(int id, [FromBody] List<Jugador> jugadores)
{
    // Buscar el partido a actualizar
    var partidoToUpdate = _context.Partidos
                                  .Include(p => p.jugadores)
                                  .FirstOrDefault(p => p.id == id);

    if (partidoToUpdate == null)
    {
        return NotFound();
    }

    foreach (var jugador in jugadores)
    {
        // Buscar el jugador existente en el partido por número de camiseta
        var existingJugador = partidoToUpdate.jugadores
                                             .FirstOrDefault(j => j.numeroCamiseta == jugador.numeroCamiseta);
        if (existingJugador != null)
        {
            // Actualizar solo las propiedades específicas del jugador existente
            existingJugador.goles = jugador.goles;
            existingJugador.partidosJugados = jugador.partidosJugados;
        }
        else
        {
            // Buscar el jugador en la base de datos por número de camiseta
            var jugadorFromDb = _context.Jugadores
                                        .FirstOrDefault(j => j.numeroCamiseta == jugador.numeroCamiseta);
            if (jugadorFromDb != null)
            {
                // Añadir el jugador encontrado al partido con todas las propiedades necesarias
                partidoToUpdate.jugadores.Add(new Jugador
                {
                    numeroCamiseta = jugadorFromDb.numeroCamiseta,
                    nombre = jugadorFromDb.nombre,
                    apellidos = jugadorFromDb.apellidos,
                    pie = jugadorFromDb.pie,
                    posicion = jugadorFromDb.posicion, // Incluyendo 'posicion'
                    goles = jugador.goles,
                    partidosJugados = jugador.partidosJugados,
                    // Añadir otras propiedades necesarias que no permitan nulos
                });
            }
            else
            {
                // Manejo si el jugador no se encuentra en la base de datos (opcional)
                return BadRequest($"Jugador con numeroCamiseta {jugador.numeroCamiseta} no encontrado en la base de datos.");
            }
        }
    }

    // Guardar los cambios en la base de datos
    _context.SaveChanges();

    return NoContent();
}


      [HttpDelete("{id}")]
public IActionResult DeletePartido(int id)
{
    var partido = _context.Partidos
                        .Include(p => p.jugadores)
                        .SingleOrDefault(p => p.id == id);

    if (partido == null)
    {
        return NotFound();
    }

    _context.Jugadores.RemoveRange(partido.jugadores);
    _context.Partidos.Remove(partido);
    _context.SaveChanges();

    return NoContent();
}


    }


}
