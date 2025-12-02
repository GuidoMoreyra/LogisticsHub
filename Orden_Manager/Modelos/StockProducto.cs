namespace Orden_Manager.Modelos;

public class StockProducto
{
    private readonly Producto producto;
    private readonly String variante;
    private int cantidad;
    private Boolean estaActivo = true;

    public int VerificarDisponibilidad(int cantidadParaVerificar)
    {
        return cantidad - cantidadParaVerificar; 
    }
}