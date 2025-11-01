using System;

public class BoletoFrecuente
{
    private int viajesEsteMes;
    private DateTime mesActual;

    public BoletoFrecuente()
    {
        viajesEsteMes = 0;
        mesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }

    public int ViajesEsteMes
    {
        get
        {
            VerificarCambioMes();
            return viajesEsteMes;
        }
    }

    public void RegistrarViaje()
    {
        VerificarCambioMes();
        viajesEsteMes++;
    }

    public void ReiniciarConteoMes()
    {
        viajesEsteMes = 0;
        mesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }

    private void VerificarCambioMes()
    {
        DateTime primerDiaMesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        if (primerDiaMesActual != mesActual)
        {
            ReiniciarConteoMes();
        }
    }

    public decimal AplicarDescuentoFrecuente(decimal tarifaBase)
    {
        VerificarCambioMes();

        if (viajesEsteMes >= 30 && viajesEsteMes <= 59)
        {
            return tarifaBase * 0.8m; // 20% descuento
        }
        else if (viajesEsteMes >= 60 && viajesEsteMes <= 80)
        {
            return tarifaBase * 0.75m; // 25% descuento
        }
        else
        {
            return tarifaBase; // Tarifa normal
        }
    }
}