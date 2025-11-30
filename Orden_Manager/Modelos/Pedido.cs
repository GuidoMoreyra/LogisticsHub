namespace Orden_Manager.Modelos;

public class Pedido
{
    private readonly long nmroDePedido;
    private readonly DateTime fechaDelPedido;
    private readonly Cliente cliente;
    private readonly List<LineaDePedido> pedido;
    private readonly decimal valorTotal;

    

    public Pedido (List<LineaDePedido> pedido, Cliente cliente)
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
        this.pedido = pedido;
        this.cliente = cliente;
        valorTotal = pedido
            .Select(p => p.producto)
            .Select(p => p.getPrecio())
            .Sum();
    }


}