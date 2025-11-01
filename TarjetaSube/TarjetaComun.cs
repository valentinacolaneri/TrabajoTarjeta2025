using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class TarjetaComun : Tarjeta
    {
        public TarjetaComun()
        {
            // AGREGAR: Inicializar boleto frecuente
            boletoFrecuente = new BoletoFrecuente();
        }

        public override decimal CalcularMontoPasaje(decimal tarifaBase)
        {
            // MODIFICAR: Aplicar descuento por uso frecuente
            return boletoFrecuente.AplicarDescuentoFrecuente(tarifaBase);
        }

        public override bool PuedePagar(decimal tarifaBase)
        {
            // MODIFICAR: Usar el monto calculado con descuentos
            decimal montoPasaje = CalcularMontoPasaje(tarifaBase);
            return saldo - montoPasaje >= -1200m;
        }
    }
}
