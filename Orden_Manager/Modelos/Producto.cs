namespace Orden_Manager.Modelos;

public class Producto
{
    private long codigoBase;
    private string nombre;
    private string descripcion;
    private string urlImg;
    private List<VarianteColor> variantes = new();

    public long GetCodigo() => codigoBase;

    public VarianteColor? GetVariante(string skuColor) => variantes.FirstOrDefault(v => v.Sku == skuColor);

    public int RestarStock(string skuColor, string talle, int cantidad)
    {
        var v = GetVariante(skuColor);
        return v?.RestarStock(talle, cantidad) ?? cantidad;
    }

    public void SumarStock(string skuColor, string talle, int cantidad)
    {
        GetVariante(skuColor)?.SumarStock(talle, cantidad);
    }
}