namespace Orden_Manager.Modelos;

public class Pedido
{   
    public const string SinVariante = "DEFAULT";
    private long nmroDePedido;
    private DateTime fechaDelPedido;
    private Cliente cliente;
    private List<LineaDePedido> pedido;
    private List<Faltante> faltantes = new();
    private decimal valorTotal;
    private long nmroDeRemito;

    public Pedido(List<LineaDePedido> pedido, Cliente cliente)
    {
        //El pedido no deberia existir sin sus lineas ni sus clientes.
        if (pedido == null) throw new ArgumentNullException(nameof(pedido));
        if (cliente == null) throw new ArgumentNullException(nameof(cliente));

        //Asigno pedido y sus clientes
        this.pedido = pedido;
        this.cliente = cliente;

        //Calculo el valor total del pedido
        valorTotal = pedido.Sum(linea => linea.CalcularValor());
        fechaDelPedido = DateTime.Today;
    }

    public void AgregarProducto(Producto producto, Dictionary<string?, int> cantidades)
    {
        //Primero verifico si ya hay alguna linea con este producto.
        LineaDePedido? linea = pedido.FirstOrDefault(l => l.GetProducto().GetCodigo() == producto.GetCodigo());

        //Si encontro la linea entonces le digo que agregue las variantes y sus cantidades
        //Luego le digo al producto que reste el stock en las variantes dadas.
        if (linea is not null)
        {
            Faltante? faltante = faltantes.FirstOrDefault(f => f.esDe(linea));
            if (faltante == null) return;

            foreach (var elemento in cantidades)
            {
                string variante = elemento.Key ?? SinVariante;
                int cantidad = elemento.Value;
                linea.SumarVariante(variante, cantidad, faltante);
            }
            //Recalculo el valor total del pedido
            valorTotal = pedido.Sum(l => l.CalcularValor());
        } 
        //Si no encuentro una linea entonces creo una nueva con los parametros dados.
        else
        {
            linea = new LineaDePedido(this, producto, cantidades);
            pedido.Add(linea);
            faltantes.Add(linea.DescontarStock());
        }
    }
    
    public void RestarVariante(string? variante, int cantidad, Producto producto)
    {
        string v = variante ?? SinVariante;
        LineaDePedido? lineaDePedido = pedido.FirstOrDefault(p => p.GetProducto().Equals(producto));
        if (lineaDePedido == null) return;

        lineaDePedido.RestarVariante(v, cantidad);
        Faltante? faltante = faltantes.FirstOrDefault(f => f.esDe(lineaDePedido));
        
        if (faltante != null && faltante.GetFaltante().ContainsKey(v))
            faltante.RestarFaltante(v, cantidad);
    }

    public void EliminarProducto(Producto producto, string? variante)
    {
        LineaDePedido? lineaDePedido = pedido.FirstOrDefault(p => p.GetProducto().Equals(producto));
        if (lineaDePedido == null) return;

        Faltante? faltante = faltantes.FirstOrDefault(f => f.esDe(lineaDePedido));

        if (variante == null)
        {
            pedido.Remove(lineaDePedido);
            if (faltante != null) faltantes.Remove(faltante);
            return;
        }

        if (lineaDePedido.GetVariantes().Contains(variante))
            lineaDePedido.EliminarVariante(variante);
        
        if (faltante != null && faltante.GetFaltante().ContainsKey(variante))
            faltante.EliminarVariante(variante);
    }
    
    public Cliente GetCliente() => cliente;
}