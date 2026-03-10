namespace Orden_Manager.Modelos;

public class VarianteColor
{
    public string Sku { get; set; }
    public string Color { get; set; }
    public decimal PrecioBase { get; set; }
    private Dictionary<string, int> stockPorTalle = new();
    private Dictionary<string, decimal> recargosPorTalle = new(); // Ej: "50-56" -> 10.0 (porcentaje)

    public int RestarStock(string talle, int cantidad)
    {
        if (!stockPorTalle.ContainsKey(talle)) return cantidad;
        int disponible = stockPorTalle[talle];
        int aRestar = Math.Min(disponible, cantidad);
        stockPorTalle[talle] -= aRestar;
        return cantidad - aRestar;
    }

    public void SumarStock(string talle, int cantidad)
    {
        if (stockPorTalle.ContainsKey(talle)) stockPorTalle[talle] += cantidad;
    }

    public decimal GetPrecioTalle(string talle)
    {
        decimal recargo = recargosPorTalle.GetValueOrDefault(talle, 0);
        return PrecioBase * (1 + (recargo / 100));
    }
}