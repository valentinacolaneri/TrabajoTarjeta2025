    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace TarjetaSube
    {
    public class MedioBoletoEstudiantil : Tarjeta
    {
        public override decimal CalcularMontoPasaje(decimal tarifaBase)
        {
            return tarifaBase / 2;
        }

        public override bool PuedePagar(decimal tarifaBase)
        {
            decimal montoPasaje = CalcularMontoPasaje(tarifaBase);
            return saldo - montoPasaje >= -1200m;
        }
    }
}
