using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class Colectivo
    {
        private const decimal TARIFA_BASICA = 1580m;
        private string linea;

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public virtual decimal ObtenerTarifa()
        {
            return TARIFA_BASICA;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            // VERIFICAR FRANJA HORARIA para franquicias
            if (!tarjeta.EstaEnFranjaHorariaPermitida())
            {
                return CrearBoletoInvalido(tarjeta, ObtenerTarifa(), "Fuera de franja horaria");
            }

            decimal tarifaBase = ObtenerTarifa();
            decimal montoPasaje = tarjeta.CalcularMontoPasaje(tarifaBase);
            decimal saldoAnterior = tarjeta.Saldo;

            // VERIFICAR TRASBORDO
            bool esTrasbordo = tarjeta.PuedeRealizarTrasbordo(linea);
            if (esTrasbordo)
            {
                montoPasaje = 0; // Trasbordo gratuito
            }

            if (tarjeta.PuedePagar(montoPasaje))
            {
                bool descuentoExitoso = tarjeta.Descontar(montoPasaje);

                if (descuentoExitoso)
                {
                    // REGISTRAR USO PARA BOLETO FRECUENTE (solo tarjeta común)
                    if (tarjeta is TarjetaComun comun)
                    {
                        comun.BoletoFrecuente.RegistrarViaje();
                    }

                    // REGISTRAR VIAJE PARA TRASBORDOS
                    tarjeta.RegistrarViajeConLinea(linea);

                    return CrearBoletoValido(tarjeta, montoPasaje, saldoAnterior, esTrasbordo);
                }
            }

            return CrearBoletoInvalido(tarjeta, montoPasaje, "No se pudo pagar");
        }

        private Boleto CrearBoletoValido(Tarjeta tarjeta, decimal montoPasaje, decimal saldoAnterior, bool esTrasbordo)
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
                montoRecarga,
                esTrasbordo
            );
        }

        private Boleto CrearBoletoInvalido(Tarjeta tarjeta, decimal montoPasaje, string motivo)
        {
            return new Boleto(
                montoPasaje,
                linea,
                DateTime.Now,
                false,
                tarjeta.GetType().Name,
                tarjeta.Saldo,
                tarjeta.Id,
                motivo: motivo
            );
        }
    }
}