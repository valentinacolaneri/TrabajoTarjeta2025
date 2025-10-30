using System;
using System.Linq;
using TarjetaSube;

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
            return tarifaBase;
        }
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        decimal montoPasaje = CalcularMontoPasaje(tarifaBase);

        // Verificar restricción de 5 SEGUNDOS entre viajes (para testing)
        if (historialViajes.Count > 0)
        {
            DateTime ultimoViaje = historialViajes.Last();
            TimeSpan tiempoDesdeUltimoViaje = DateTime.Now - ultimoViaje;

            if (tiempoDesdeUltimoViaje.TotalSeconds < 5)  // ← Cambiado de Minutes a Seconds
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