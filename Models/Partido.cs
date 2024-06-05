// Modelo de partido
public class Partido
{
    public int id { get; set; }
    public int jornadaId {get; set;}
    public string nombrePartido{ get; set; }
    public string primerEquipo{ get; set; }
    public string segundoEquipo{ get; set; }
    public int puntuacion { get; set; }
    public bool ganado{ get; set; }
    public DateTime fecha { get; set; }
    public List<Jugador> jugadores { get; set; }

}