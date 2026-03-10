using Orden_Manager.Domain.Enums;

namespace Orden_Manager.Modelos;

public class LogAuditoria
{
    public long Id { get; set; }
    
    public Usuario Usuario { get; set; }
    public DateTime Fecha { get; set; }
    //public Usuario Usuario { get; set; } TODO Todavia no se hizo la capa de  auth
    public TipoDeAccion TipoDeAccion { get; set; } 
    public Producto? Producto { get; set; } //Es null si el cambio  fue a un pedido.
    public Pedido? Pedido { get; set; } // Es null si el cambio fue a un producto.
    public String Detalle { get; set; } // Deberia aclarar donde se hizo  el cambio y cual fue
}