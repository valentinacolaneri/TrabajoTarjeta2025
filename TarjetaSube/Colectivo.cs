public class Colectivo
{
    private const decimal TARIFA_BASICA = 1580m;
    private string linea;

    public Colectivo(string linea)
    {
        this.linea = linea;
    }

    public Boleto PagarCon(Tarjeta tarjeta)
    {
        decimal montoPasaje = tarjeta.CalcularMontoPasaje(TARIFA_BASICA);

        if (tarjeta.PuedePagar(TARIFA_BASICA))
        {
            tarjeta.Descontar(montoPasaje);
            return new Boleto(montoPasaje, linea, DateTime.Now, true);
        }
        else
        {
            return new Boleto(montoPasaje, linea, DateTime.Now, false);
        }
    }
}