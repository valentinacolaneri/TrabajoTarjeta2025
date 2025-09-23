using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public abstract class Tarjeta
{
    protected decimal saldo;
    private const decimal LIMITE_SALDO = 40000m;
    private const decimal SALDO_NEGATIVO_MAXIMO = -1200m;
    protected static readonly decimal[] CARGAS_ACEPTADAS =
        { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

    public decimal Saldo => saldo;

    protected Tarjeta()
    {
        saldo = 0m;
    }

    public virtual bool Cargar(decimal monto)
    {
        if (!CARGAS_ACEPTADAS.Contains(monto))
            return false;

        if (saldo + monto > LIMITE_SALDO)
            return false;

        // Si tiene saldo negativo, primero descuenta la deuda
        if (saldo < 0)
        {
            decimal deuda = Math.Abs(saldo);
            decimal montoRestante = monto - deuda;

            if (montoRestante >= 0)
            {
                saldo = montoRestante;
                return true;
            }
            else
            {
                saldo += monto;
                return true;
            }
        }
        else
        {
            saldo += monto;
            return true;
        }
    }

    public virtual bool Descontar(decimal monto)
    {
        decimal nuevoSaldo = saldo - monto;

        if (nuevoSaldo < SALDO_NEGATIVO_MAXIMO)
            return false;

        saldo = nuevoSaldo;
        return true;
    }

    public abstract decimal CalcularMontoPasaje(decimal tarifaBase);
    public abstract bool PuedePagar(decimal tarifaBase);
}
    }