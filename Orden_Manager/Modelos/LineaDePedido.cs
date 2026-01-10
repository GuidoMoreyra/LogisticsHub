namespace Orden_Manager.Modelos;

public class LineaDePedido
{
    private Pedido pedido;
    private Producto producto;
    private VarianteColor variante;
    private Dictionary<string, int> cantidadesPorTalle = new(); // Key: Talle, Value: Cantidad

    public LineaDePedido(Pedido pedido, Producto producto, VarianteColor variante, Dictionary<string, int> tallesCantidades)
    {
        this.pedido = pedido;
        this.producto = producto;
        this.variante = variante;
        this.cantidadesPorTalle = tallesCantidades;
    }
    
    // Getters necesarios para la lógica de Pedido
    public Producto GetProducto() => producto;
    public VarianteColor GetVariante() => variante;
    public List<string> GetTalles() => cantidadesPorTalle.Keys.ToList();

    public decimal CalcularValor()
    {
        return cantidadesPorTalle.Sum(kvp => variante.GetPrecioTalle(kvp.Key) * kvp.Value);
    }

    public Faltante DescontarStock()
    {
        var faltantesReportados = new Dictionary<string, int>();
        foreach (var kvp in cantidadesPorTalle)
        {
            int faltante = variante.RestarStock(kvp.Key, kvp.Value);
            if (faltante > 0) 
                faltantesReportados.Add($"{variante.Color} - Talle: {kvp.Key}", faltante);
        }
        return new Faltante(this, this.pedido, this.pedido.GetCliente(), faltantesReportados);
    }

    public void SumarVariante(string talle, int cantidad, Faltante faltante)
    {
        int cantFaltante = variante.RestarStock(talle, cantidad);
        
        if (cantidadesPorTalle.ContainsKey(talle))
            cantidadesPorTalle[talle] += cantidad;
        else
            cantidadesPorTalle.Add(talle, cantidad);

        if (cantFaltante > 0)
            faltante.SumarFaltante($"{variante.Color} - Talle: {talle}", cantFaltante);
    }

    public void RestarVariante(string talle, int cantidad, Faltante? faltante)
    {
        if (cantidadesPorTalle.ContainsKey(talle))
        {
            cantidadesPorTalle[talle] -= cantidad;
            variante.SumarStock(talle, cantidad);
            
            if (faltante != null)
                faltante.RestarFaltante($"{variante.Color} - Talle: {talle}", cantidad);
            
            // Si la cantidad llega a 0, podrías evaluar si remover el talle del diccionario
            if (cantidadesPorTalle[talle] <= 0) cantidadesPorTalle.Remove(talle);
        }
    }

    public void EliminarTalle(string talle)
    {
        if (cantidadesPorTalle.ContainsKey(talle))
        {
            variante.SumarStock(talle, cantidadesPorTalle[talle]);
            cantidadesPorTalle.Remove(talle);
        }
    }
}