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
        }
    
}
