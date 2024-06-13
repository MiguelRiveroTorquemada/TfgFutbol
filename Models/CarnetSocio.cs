public class CarnetSocio
{
    public int id { get; set; }
    public int precio { get; set; }
    public Cliente cliente { get; set; }
    // Otros atributos del carnet de socio
   public string StripeCustomerId { get; set; }
    public string StripeSessionId { get; set; } 

}