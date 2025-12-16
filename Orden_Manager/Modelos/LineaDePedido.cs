using Microsoft.VisualBasic.CompilerServices;

namespace Orden_Manager.Modelos;

public class LineaDePedido
{
    private Producto producto;
    private Dictionary<String?, int> cantidades_;

    public Producto GetProducto()
    {
        return producto;
    }
    public List<String>? GetVariantes()
    {
        return cantidades_?.Keys.ToList();
    }

    public void DescontarStock()
    {
        List<String> variantes = cantidades_.Keys.ToList();
        variantes.ForEach(variante => producto.RestarStock(variante, cantidades_[variante])
        );

    }

    private void DescontarStock(String variante, int cantidad)
    {
        producto.RestarStock(variante, cantidad);
    }

    public void SumarVariante(String variante, int cantidad)                                                        
    {
        cantidades_.Add(variante, cantidad);
        DescontarStock(variante, cantidad);
    }
}