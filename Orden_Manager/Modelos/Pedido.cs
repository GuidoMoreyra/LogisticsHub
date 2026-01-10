namespace Orden_Manager.Modelos;

public class Pedido
{   
    private long nmroDePedido;
    private DateTime fechaDelPedido;
    private Cliente cliente;
    private List<LineaDePedido> pedido = new();
    private List<Faltante> faltantes = new();
    private decimal valorTotal;
    private long nmroDeRemito;

    public Pedido(List<LineaDePedido> lineasIniciales, Cliente cliente)
    {
        if (lineasIniciales == null) throw new ArgumentNullException(nameof(lineasIniciales));
        this.cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));

        this.pedido = lineasIniciales;
        ActualizarValorTotal();
        fechaDelPedido = DateTime.Today;
    }

    public void AgregarProducto(Producto producto, VarianteColor variante, Dictionary<string, int> tallesCantidades)
    {
        // Buscamos la línea que coincida con el producto Y la variante de color específica
        LineaDePedido? linea = pedido.FirstOrDefault(l => 
            l.GetProducto().GetCodigo() == producto.GetCodigo() && 
            l.GetVariante().Sku == variante.Sku);

        if (linea is not null)
        {
            Faltante? faltante = faltantes.FirstOrDefault(f => f.EsDe(linea));
            // Si por alguna razón no hay objeto faltante, lo creamos para evitar nulos
            if (faltante == null) {
                faltante = new Faltante(linea, this, this.cliente, new Dictionary<string, int>());
                faltantes.Add(faltante);
            }

            foreach (var item in tallesCantidades)
            {
                linea.SumarVariante(item.Key, item.Value, faltante);
            }
        } 
        else
        {
            linea = new LineaDePedido(this, producto, variante, tallesCantidades);
            pedido.Add(linea);
            faltantes.Add(linea.DescontarStock());
        }
        
        ActualizarValorTotal();
    }
    
    public void RestarVariante(Producto producto, VarianteColor variante, string talle, int cantidad)
    {
        LineaDePedido? linea = pedido.FirstOrDefault(l => 
            l.GetProducto().GetCodigo() == producto.GetCodigo() && 
            l.GetVariante().Sku == variante.Sku);

        if (linea == null) return;

        Faltante? faltante = faltantes.FirstOrDefault(f => f.EsDe(linea));
        linea.RestarVariante(talle, cantidad, faltante);
        ActualizarValorTotal();
    }

    public void EliminarProducto(Producto producto, VarianteColor variante, string? talle = null)
    {
        LineaDePedido? linea = pedido.FirstOrDefault(l => 
            l.GetProducto().GetCodigo() == producto.GetCodigo() && 
            l.GetVariante().Sku == variante.Sku);

        if (linea == null) return;

        Faltante? faltante = faltantes.FirstOrDefault(f => f.EsDe(linea));

        if (talle == null)
        {
            // Eliminar la línea completa (todos los talles de ese color)
            // Es necesario devolver el stock de todos los talles antes de remover
            foreach (var t in linea.GetTalles()) 
            {
                linea.EliminarTalle(t);
            }
            pedido.Remove(linea);
            if (faltante != null) faltantes.Remove(faltante);
        }
        else
        {
            // Eliminar solo un talle específico de esa variante de color
            linea.EliminarTalle(talle);
            
            // Limpiar reporte de faltante para ese talle si existía
            string claveFaltante = $"{variante.Color} - Talle: {talle}";
            if (faltante != null && faltante.GetFaltante().ContainsKey(claveFaltante))
                faltante.EliminarVariante(claveFaltante);
        }

        ActualizarValorTotal();
    }

    private void ActualizarValorTotal() => valorTotal = pedido.Sum(l => l.CalcularValor());
    
    public Cliente GetCliente() => cliente;
}