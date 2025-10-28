using System;
using System.Linq;

public class MedioBoletoEstudiantil : Tarjeta
{
    public override decimal CalcularMontoPasaje(decimal tarifaBase)
    {
        if (PuedeViajarMedioBoleto())
        {
            return tarifaBase / 2;
        }
        else
        {
            // Después del segundo viaje del día, paga tarifa completa
            return tarifaBase;
        }
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        decimal montoPasaje = CalcularMontoPasaje(tarifaBase);

        // Verificar restricciones de tiempo
        if (historialViajes.Count > 0)
        {
            DateTime ultimoViaje = historialViajes.Last();
            TimeSpan tiempoDesdeUltimoViaje = DateTime.Now - ultimoViaje;

            if (tiempoDesdeUltimoViaje.TotalMinutes < 5)
                return false;
        }

        return saldo - montoPasaje >= -1200m;
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