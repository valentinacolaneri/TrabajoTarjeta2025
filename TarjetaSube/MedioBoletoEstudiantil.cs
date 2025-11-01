using System;
using System.Linq;
using TarjetaSube;

public class MedioBoletoEstudiantil : Tarjeta
{
    public override decimal CalcularMontoPasaje(decimal tarifaBase)
    {
        return PuedeViajarMedioBoleto() ? tarifaBase / 2 : tarifaBase;
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        decimal montoPasaje = CalcularMontoPasaje(tarifaBase);

        // ✅ AGREGAR ESTA VERIFICACIÓN CRÍTICA:
        // Si no puede viajar con medio boleto, tampoco puede pagar (para los primeros 2 viajes)
        if (!PuedeViajarMedioBoleto() && CantidadViajesHoy() < 2)
            return false;

        return Saldo - montoPasaje >= -1200m;
    }
   
}