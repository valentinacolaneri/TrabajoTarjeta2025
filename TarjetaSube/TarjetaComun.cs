using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
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
}
