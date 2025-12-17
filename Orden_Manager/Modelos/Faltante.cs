namespace Orden_Manager.Modelos;

public class Faltante
{
    private LineaDePedido _lineaConFaltante;
    private Dictionary<string, int> _faltantes;
    private Pedido _pedido;
    private Cliente _cliente;

    public Faltante(LineaDePedido lineaConFaltante, Pedido pedido, Cliente cliente, Dictionary<string, int> faltantes)
    {
        this._lineaConFaltante = lineaConFaltante;
        this._pedido = pedido;
        this._cliente = cliente;
        this._faltantes = faltantes;
    }
    
    public Dictionary<string, int> GetFaltante() => _faltantes;

    public bool esDe(LineaDePedido linea) => _lineaConFaltante.Equals(linea);
}