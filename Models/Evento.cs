public class Evento
{
    public int id { get; set; }
    public string nombre { get; set; }
    public DateTime fecha { get; set; }
    public string lugar { get; set; }
    public bool asiste { get; set; }
    // Otros campos relacionados con el evento, como descripción, categoría, etc.
    public List<Usuario> usuarios { get; set; }
}
