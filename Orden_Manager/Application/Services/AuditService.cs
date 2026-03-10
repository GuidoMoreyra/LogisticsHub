using Orden_Manager.Domain.Enums;
using Orden_Manager.Modelos;

namespace Orden_Manager.Application.Services;

public class AuditService
{
    //private readonly DbContext context { get; set; }
    
    public void Registrar(Usuario usuario,Producto producto,Pedido pedido,TipoDeAccion tipoDeAccion,String detalle)
    {
        LogAuditoria auditoria;
        DateTime fecha = DateTime.Now;
        if (producto.Equals(null))
        {
            if (tipoDeAccion == TipoDeAccion.AjusteStock)
            {
                throw new ArgumentException("La accion no coincide con el tipo de objetivo modificado.");
            }

            auditoria = new LogAuditoria
            {
                Fecha = fecha,
                Usuario = usuario,
                TipoDeAccion = tipoDeAccion,
                Producto = null,
                Pedido = pedido,
                Detalle = detalle
            };
            
        }
        else
        {
           auditoria = new LogAuditoria
            {
                Fecha = fecha,
                Usuario = usuario,
                TipoDeAccion = tipoDeAccion,
                Producto = producto,
                Pedido = null,
                Detalle = detalle
            };
        }
        AuthRepository repositorio = new AuthRepository(context_);
        repositorio.persist(auditoria);
    }
}