namespace Orden_Manager.Modelos;

public class Cliente
{   
    private string nombreCompleto;
    private string direccion;
    private string localidad;
    private long cuit;
    private string expreso;

    public void SetNombre(string nombre)
    {
        this.nombreCompleto = nombre;
    }

    public void SetDireccion(string direccion)
    {
        this.direccion = direccion;
    }

    public void SetLocalidad(string localidad)
    {
        this.localidad = localidad;
    }

    private void SetCuit(long cuit)
    {
        this.cuit = cuit;
    }

    private void SetExpreso(string expreso)
    {
        this.expreso = expreso;
    }
}