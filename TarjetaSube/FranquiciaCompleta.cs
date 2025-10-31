using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class FranquiciaCompleta : Tarjeta
    {
        public override decimal CalcularMontoPasaje(decimal tarifaBase)
        {
            return 0m;
        }

        public override bool PuedePagar(decimal tarifaBase)
        {
            return true; // Siempre puede pagar
        }

        public new bool Descontar(decimal monto)
        {
            // Para franquicia completa, no se descuenta nada del saldo
            // pero se registra el viaje
            RegistrarViaje();
            return true;
        }
    }
}
