namespace Orden_Manager.Modelos;

public class Producto
{
    private String nombre;
    private String descripcion;
    private String urlImg;
    private decimal precio;
    private long codigo;

    public decimal getPrecio()
    {
        return precio;
    }
}