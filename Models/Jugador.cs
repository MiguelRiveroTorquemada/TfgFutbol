// Modelo de jugador
public class Jugador
{
    public int id { get; set; }
    public string nombre { get; set; }
    public string apellidos{ get; set; }
    public string posicion { get; set; }
    public decimal? altura { get; set; }
    public string pie{ get; set; }
 
    public int numeroCamiseta { get; set; }
    public bool partidosJugados { get; set; }
    public int goles { get; set; }
    public decimal golesPorPartido { get; set; } 

}