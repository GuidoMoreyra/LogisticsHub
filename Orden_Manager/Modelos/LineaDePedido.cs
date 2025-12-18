namespace Orden_Manager.Modelos;

public class LineaDePedido
{
    private Pedido pedido;
    private Producto producto;
    private Dictionary<string, int> cantidades_ = new();

    public LineaDePedido(Pedido pedido, Producto producto, Dictionary<string?, int> cantidades)
    {
        this.pedido = pedido;
        this.producto = producto;
        
        foreach (var kvp in cantidades)
        {
            this.cantidades_.Add(kvp.Key ?? Pedido.SinVariante, kvp.Value);
        }
    }

    public Producto GetProducto() => producto;

    public List<string> GetVariantes() => cantidades_.Keys.ToList();

    public Faltante DescontarStock()
    {
        var faltantesReportados = new Dictionary<string, int>();

        foreach (var kvp in cantidades_)
        {
            string variante = kvp.Key;
            int cantidadSolicitada = kvp.Value;
            
            int cantidadFaltante = producto.RestarStock(variante, cantidadSolicitada);

            if (cantidadFaltante > 0)
            {
                faltantesReportados.Add(variante, cantidadFaltante);
            }
        }
        return new Faltante(this, this.pedido, this.pedido.GetCliente(), faltantesReportados);
    }

    public void SumarVariante(string variante, int cantidad, Faltante faltante)                                                      
    {
        //Resto el producto
        int cantidadFaltante = producto.RestarStock(variante, cantidad);

        // Actualizar el diccionario de la línea y su faltante si corresponde
        if (cantidades_.ContainsKey(variante))
        {  
            cantidades_[variante] += cantidad;
            if (cantidadFaltante > 0)
                faltante.SumarFaltante(variante, cantidadFaltante);
        }
        else 
        {
            cantidades_.Add(variante, cantidad);
            if (cantidadFaltante > 0)
                faltante.AñadirFaltante(variante, cantidadFaltante);
        }
    }

    public void RestarVariante(String variante, int cantidad, Faltante? faltante)
    {
        cantidades_[variante] -= cantidad;
        producto.SumarStock(variante, cantidad);
        if(faltante != null)
            faltante.RestarFaltante(variante, cantidad);
        
    }
    public decimal CalcularValor()
    {
        decimal cantidadesTotal = cantidades_.Values.Sum();
        return producto.GetPrecio() * cantidadesTotal;
    }

    public void EliminarVariante(String variante)
    {
        producto.SumarStock(variante, cantidades_[variante]);
        cantidades_.Remove(variante);
    }
}