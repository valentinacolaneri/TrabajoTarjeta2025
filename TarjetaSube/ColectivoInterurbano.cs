using TarjetaSube;

public class ColectivoInterurbano : Colectivo
{
    private const decimal TARIFA_INTERURBANA = 3000m;

    public ColectivoInterurbano(string linea) : base(linea)
    {
    }

    public override decimal ObtenerTarifa()
    {
        return TARIFA_INTERURBANA;
    }
}
