public class TarjetaComun : Tarjeta
{
    public override decimal CalcularMontoPasaje(decimal tarifaBase)
    {
        return tarifaBase;
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        return saldo - tarifaBase >= -1200m;
    }
}
