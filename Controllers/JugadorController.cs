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
    public class JugadorController : ControllerBase
    {
        private readonly DataContext _context;

        public JugadorController(DataContext dataContext)
        {
            _context = dataContext;
        }

[HttpGet]
public ActionResult<List<Jugador>> Get()
{
    // Obtener todos los jugadores de la base de datos
    var jugadores = _context.Jugadores.ToList();

    // Crear un diccionario para almacenar la suma de goles por número de camiseta
    var sumaGolesPorNumeroCamiseta = new Dictionary<int, (int totalGoles, int partidosJugados)>();

    // Iterar sobre los jugadores para sumar los goles por número de camiseta y contar los partidos jugados
    foreach (var jugador in jugadores)
    {
        if (!sumaGolesPorNumeroCamiseta.ContainsKey(jugador.numeroCamiseta))
        {
            // Si es la primera vez que encontramos este número de camiseta, inicializar la suma de goles y el contador de partidos jugados
            sumaGolesPorNumeroCamiseta[jugador.numeroCamiseta] = (jugador.goles, jugador.partidosJugados ? 1 : 0);
        }
        else
        {
            // Si ya hemos encontrado este número de camiseta, sumar los goles al total existente y actualizar el contador de partidos jugados
            var (totalGoles, partidosJugados) = sumaGolesPorNumeroCamiseta[jugador.numeroCamiseta];
            sumaGolesPorNumeroCamiseta[jugador.numeroCamiseta] = (totalGoles + jugador.goles, partidosJugados + (jugador.partidosJugados ? 1 : 0));
        }
    }

    // Actualizar los goles por partido de los jugadores en la lista original
    foreach (var jugador in jugadores)
    {
        var (totalGoles, partidosJugados) = sumaGolesPorNumeroCamiseta[jugador.numeroCamiseta];
        jugador.goles = totalGoles;
        jugador.golesPorPartido = partidosJugados > 0 ? (decimal)totalGoles / partidosJugados : 0;
    }

    // Actualizar el ID de los jugadores con el mismo número de camiseta para que solo se muestre el jugador con el ID más alto
    var jugadoresPorNumeroCamiseta = new Dictionary<int, Jugador>();
    foreach (var jugador in jugadores)
    {
        if (!jugadoresPorNumeroCamiseta.ContainsKey(jugador.numeroCamiseta))
        {
            jugadoresPorNumeroCamiseta[jugador.numeroCamiseta] = jugador;
        }
        else
        {
            var existingJugador = jugadoresPorNumeroCamiseta[jugador.numeroCamiseta];
            if (jugador.id > existingJugador.id)
            {
                jugadoresPorNumeroCamiseta[jugador.numeroCamiseta] = jugador;
            }
        }
    }

    // Convertir el diccionario de jugadores nuevamente en una lista
    var jugadoresFinales = jugadoresPorNumeroCamiseta.Values.ToList();

    return Ok(jugadoresFinales);
}


        [HttpGet("GetByName")]
        public ActionResult<List<Jugador>> GetByName(string nombre)
        {
            // Filtrar jugadores por nombre
            var jugadores = _context.Jugadores
                .Where(j => j.nombre.Contains(nombre))
                .ToList();

            // Verificar si se encontraron jugadores
            if (jugadores.Count == 0)
            {
                return NotFound();
            }

            return Ok(jugadores);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Jugador jugador)
        {
            try
            {
                // Verificar si ya existe un jugador con el mismo número de camiseta
                var jugadorExistente = _context.Jugadores.FirstOrDefault(j => j.numeroCamiseta == jugador.numeroCamiseta);

                if (jugadorExistente != null)
                {
                    // Si existe, asignar el mismo ID al nuevo jugador
                    jugador.id = jugadorExistente.id;
                }

                // Guardar el jugador en la base de datos
                _context.Jugadores.Add(jugador);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
[HttpDelete("{numeroCamiseta}")]
public ActionResult Delete(int numeroCamiseta)
{
    try
    {
        // Buscar todos los jugadores con el número de camiseta 30
        var jugadores = _context.Jugadores.Where(j => j.numeroCamiseta == numeroCamiseta).ToList();

        if (jugadores.Count == 0)
        {
            return NotFound($"No se encontraron jugadores con número de camiseta {numeroCamiseta}.");
        }

        // Eliminar los jugadores de la base de datos
        _context.Jugadores.RemoveRange(jugadores);
        _context.SaveChanges();

        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
    }
}

[HttpPut("{id}")]
public ActionResult UpdateJugador(int id, [FromBody] Jugador jugadorUpdateDto)
{
    if (jugadorUpdateDto == null)
    {
        return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
    }

    try
    {
        // Buscar el usuario por ID
        Jugador jugador = _context.Jugadores.Find(id);

        if (jugador == null)
        {
            return NotFound($"Usuario con ID {id} no encontrado.");
        }

        // Actualizar solo si se proporcionan nuevos valores
        if (!string.IsNullOrEmpty(jugadorUpdateDto.nombre))
        {
            jugador.nombre = jugadorUpdateDto.nombre;
        }
        if (!string.IsNullOrEmpty(jugadorUpdateDto.apellidos))
        {
            jugador.apellidos = jugadorUpdateDto.apellidos;
        }
        if (!string.IsNullOrEmpty(jugadorUpdateDto.posicion))
        {
            jugador.posicion = jugadorUpdateDto.posicion;
        }
        if (jugadorUpdateDto.altura.HasValue)
        {
            jugador.altura = jugadorUpdateDto.altura.Value;
        }
        if (!string.IsNullOrEmpty(jugadorUpdateDto.pie))
        {
            jugador.pie = jugadorUpdateDto.pie;
        }

        // Guardar los cambios en la base de datos
        _context.SaveChanges();

        return Ok($"Usuario con ID {id} actualizado exitosamente.");
    }
    catch (Exception ex)
    {
        // Logging del error
        Console.WriteLine($"Error: {ex.Message}");
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

    }
    }
