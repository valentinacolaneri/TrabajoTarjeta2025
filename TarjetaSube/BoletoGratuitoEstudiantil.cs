public class BoletoGratuitoEstudiantil : Tarjeta
{
    public override decimal CalcularMontoPasaje(decimal tarifaBase)
    {
        if (PuedeViajarGratuito())
        {
            return 0m; // Gratuito para los primeros 2 viajes
        }
        else
        {
            return tarifaBase; // Tarifa completa después del segundo viaje
        }
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        // Siempre puede pagar, pero el monto cambia
        return true;
    }

    public new bool Descontar(decimal monto)
    {
        bool resultado = base.Descontar(monto);
        if (resultado)
        {
            RegistrarViaje();
        }
        return resultado;
    }
}