namespace Orden_Manager.Modelos;

public class Pedido
{   
    
    private long nmroDePedido;
    private DateTime fechaDelPedido;
    private Cliente cliente;
    private List<LineaDePedido> pedido;
    private decimal valorTotal;
    private long nmroDeRemito;



    public Pedido(List<LineaDePedido> pedido, Cliente cliente)
    {
        //El pedido no deberia existir sin sus lineas ni sus clientes.
        if (pedido == null)
        {
            throw new ArgumentNullException(nameof(pedido));
        }

        if (cliente == null)
        {
            throw new ArgumentNullException(nameof(cliente));
        }

        //Asigno pedido y sus clientes
        this.pedido = pedido;
        this.cliente = cliente;

        //Calculo el valor total del pedido
        valorTotal = CalcularValorTotal(); //Hay que hacer otro metodo en LineaPedido para obtener el precio

        fechaDelPedido = DateTime.Today;

        pedido.ForEach(linea => linea.DescontarStock());
    }

    //Se crea la linea , se suma su valor al total y se descuenta su stock
    public void AgregarProducto (Producto producto, Dictionary<String, int>? cantidades)
    {
        //Primero verifico si ya hay alguna linea con este producto.
        LineaDePedido? linea = null;
        foreach (LineaDePedido lineaDePedido in pedido)
        {
            long codigoDelProducto = lineaDePedido.GetProducto().GetCodigo();
            if (codigoDelProducto == producto.GetCodigo())
            {
                linea = lineaDePedido;
                break;
            }
        }
        
        //Si encontro la linea entonces le digo que agregue las variantes y sus cantidades
        //Luego le digo al producto que reste el stock en las variantes dadas.
        if (linea is not null)
        {
            foreach (KeyValuePair<String, int> elemento in cantidades)
            {
                string variante = elemento.Key;
                int cantidad = elemento.Value;
                linea.SumarVariante(variante , cantidad);
                // linea.producto.RestarStock(variante, cantidad); /* Deberia hacerlo la linea. */
            }
            //Recalculo el valor total del pedido
            CalcularValorTotal();
        } 
        //Si no encuentro una linea entonces creo una nueva con los parametros dados.
        else
        {
            linea = new LineaDePedido
            {
                producto = producto,
                cantidades = cantidades,
            };
            pedido.Add(linea);
        }
        
        
    }

    public void EliminarProducto(Producto producto, String? variante)
    {
        foreach (LineaDePedido lineaDePedido in pedido)
        {
            if (lineaDePedido.GetProducto().GetCodigo() == producto.GetCodigo())
            {
                if (variante is null)
                {
                    pedido.Remove(lineaDePedido);
                    break;
                }
                //producto.SumarStock(cantidades[variante], variante); Deberia hacerlo la linea
                lineaDePedido.EliminarVariante(variante);
            }
        }
    }


}