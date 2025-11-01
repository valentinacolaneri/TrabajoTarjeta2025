using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class BoletoGratuitoEstudiantil : Tarjeta
    {
        public override decimal CalcularMontoPasaje(decimal tarifaBase)
        {
            return 0m;
        }

        public override bool PuedePagar(decimal tarifaBase)
        {
            return true; // Siempre puede pagar
        }

        // === NUEVO MÉTODO PARA FRANJA HORARIA ===
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
