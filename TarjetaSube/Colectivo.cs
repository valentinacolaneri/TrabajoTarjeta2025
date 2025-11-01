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
        

        if (tarjeta.PuedePagar(TARIFA_BASICA))
        {
            

            bool descuentoExitoso = tarjeta.Descontar(montoPasaje);
            

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

                var boleto = new Boleto(
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

                
                return boleto;
            }
            else
            {
                
            }
        }
        else
        {
            
        }

        // Si no se pudo pagar
        var boletoInvalido = new Boleto(
            montoPasaje,
            linea,
            DateTime.Now,
            false,
            tarjeta.GetType().Name,
            tarjeta.Saldo,
            tarjeta.Id
        );

        
        return boletoInvalido;
    }
}