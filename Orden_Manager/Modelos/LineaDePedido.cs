using Microsoft.VisualBasic.CompilerServices;

namespace Orden_Manager.Modelos;

public class LineaDePedido
{
    public Producto producto;
    public Dictionary<String, int>? cantidades;

}