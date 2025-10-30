using System;
using TarjetaSube;

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
        decimal saldoAnterior = tarjeta.Saldo;

        // ✅ NUEVA LÓGICA: Si es franquicia completa o boleto gratuito, monto = 0
        if (tarjeta is FranquiciaCompleta || tarjeta is BoletoGratuitoEstudiantil)
        {
            // Verificar si está en franja horaria permitida
            if (EstaEnFranjaHorariaPermitida())
            {
                montoPasaje = 0m; // ✅ Boleto completamente gratuito
            }
        }

        if (tarjeta.PuedePagar(TARIFA_BASICA))
        {
            bool descuentoExitoso = false;

            // ✅ LÓGICA MEJORADA: Manejar diferentes tipos de tarjeta
            if (tarjeta is MedioBoletoEstudiantil medioBoleto)
            {
                descuentoExitoso = medioBoleto.Descontar(montoPasaje);
            }
            else if (tarjeta is BoletoGratuitoEstudiantil gratuito)
            {
                // Para boleto gratuito, usar el método Descontar específico
                descuentoExitoso = gratuito.Descontar(montoPasaje);
            }
            else if (tarjeta is FranquiciaCompleta franquicia)
            {
                // Para franquicia completa, usar el método Descontar específico
                descuentoExitoso = franquicia.Descontar(montoPasaje);
            }
            else
            {
                // Para tarjeta común
                descuentoExitoso = tarjeta.Descontar(montoPasaje);
            }

            if (descuentoExitoso)
            {
                // Calcular si hubo recarga por saldo negativo
                decimal montoRecarga = 0;
                decimal totalAbonado = montoPasaje;

                if (saldoAnterior < 0)
                {
                    montoRecarga = Math.Min(Math.Abs(saldoAnterior), montoPasaje);
                    totalAbonado = montoPasaje + montoRecarga;
                }

                return new Boleto(
                    montoPasaje,
                    linea,
                    DateTime.Now,
                    true,
                    tarjeta.GetType().Name,
                    tarjeta.Saldo,
                    tarjeta.Id,
                    totalAbonado,
                    montoRecarga
                );
            }
        }

        // Si no se pudo pagar
        return new Boleto(
            montoPasaje,
            linea,
            DateTime.Now,
            false,
            tarjeta.GetType().Name,
            tarjeta.Saldo,
            tarjeta.Id
        );
    }

    // ✅ MÉTODO AUXILIAR para verificar franja horaria
    private bool EstaEnFranjaHorariaPermitida()
    {
        DateTime ahora = DateTime.Now;
        DayOfWeek dia = ahora.DayOfWeek;
        int hora = ahora.Hour;

        // Lunes a viernes de 6 a 22
        if (dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday)
        {
            return hora >= 6 && hora < 22;
        }

        return false;
    }
}