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

    public bool EsDe(LineaDePedido linea) => _lineaConFaltante.Equals(linea);

    public void AñadirFaltante(String variedad, int cantidadFaltante)
    {
        _faltantes.Add(variedad, cantidadFaltante);
    }

    public void SumarFaltante(String variedad, int cantidadFaltante)
    {
        _faltantes[variedad] += cantidadFaltante;
    }

    public void RestarFaltante(String variedad, int cantidadFaltante)
    {
        if (_faltantes.ContainsKey(variedad) && _faltantes[variedad] > 0)
        {
            _faltantes[variedad] -= cantidadFaltante;
            if (_faltantes[variedad] <= 0)
                _faltantes.Remove(variedad);
        }
    }

    public void EliminarVariante(String variante)
    {
        _faltantes.Remove(variante);
    }
    
}