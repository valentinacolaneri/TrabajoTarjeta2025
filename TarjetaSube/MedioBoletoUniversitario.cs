using System;

namespace TarjetaSube
{
    public class MedioBoletoUniversitario : Tarjeta
    {
        public override decimal CalcularMontoPasaje(decimal tarifaBase)
        {
            return tarifaBase / 2; // 50% descuento
        }

        public override bool PuedePagar(decimal tarifaBase)
        {
            decimal montoPasaje = CalcularMontoPasaje(tarifaBase);
            return saldo - montoPasaje >= -1200m;
        }

        public override bool EstaEnFranjaHorariaPermitida()
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
}
