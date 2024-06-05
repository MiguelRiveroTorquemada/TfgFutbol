public class Carrito
{
    public int id { get; set;}
    public List<Producto> items { get; set; }
    public List<CarnetSocio> carnetSocios { get; set; }
    public decimal total { get; set; }
    public List<Cliente> clientes { get; set; }

    //borrar si no funciona 
    public bool enviado { get;set;}


    // Otros atributos del carrito
}
