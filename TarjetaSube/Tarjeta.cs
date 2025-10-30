using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Tarjeta
{
    protected decimal saldo;
    protected decimal saldoPendienteAcreditacion;
    private const decimal LIMITE_SALDO = 56000m; // Actualizado de 40000 a 56000
    private const decimal SALDO_NEGATIVO_MAXIMO = -1200m;
    protected static readonly decimal[] CARGAS_ACEPTADAS =
        { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

    public decimal Saldo => saldo;
    public decimal SaldoPendienteAcreditacion => saldoPendienteAcreditacion;
    public int Id { get; protected set; }
    private static int proximoId = 1;

    // Historial de viajes para control de límites
    protected List<DateTime> historialViajes;

    protected Tarjeta()
    {
        saldo = 0m;
        saldoPendienteAcreditacion = 0m;
        Id = proximoId++;
        historialViajes = new List<DateTime>();
    }

    public virtual bool Cargar(decimal monto)
    {
        if (!CARGAS_ACEPTADAS.Contains(monto))
            return false;

        // ✅ NUEVA LÓGICA: Pagar deuda primero si hay saldo negativo
        if (saldo < 0)
        {
            decimal deuda = Math.Abs(saldo);
            decimal montoRestante = monto - deuda;

            if (montoRestante >= 0)
            {
                // ✅ Paga la deuda completa y aplica el resto al saldo
                saldo = montoRestante;

                // Verificar si el saldo restante cabe en el límite
                decimal espacioDisponible = LIMITE_SALDO - saldo;
                if (montoRestante <= espacioDisponible)
                {
                    saldo += montoRestante;
                }
                else
                {
                    saldo = LIMITE_SALDO;
                    saldoPendienteAcreditacion += (montoRestante - espacioDisponible);
                }
                return true;
            }
            else
            {
                // ✅ Reduce la deuda parcialmente (no alcanza para pagarla toda)
                saldo += monto;
                return true;
            }
        }
        else
        {
            // Lógica original para cuando no hay deuda
            decimal espacioDisponible = LIMITE_SALDO - saldo;

            if (monto <= espacioDisponible)
            {
                saldo += monto;
                return true;
            }
            else
            {
                saldo = LIMITE_SALDO;
                saldoPendienteAcreditacion += (monto - espacioDisponible);
                return true;
            }
        }
    }

    public virtual void AcreditarCarga()
    {
        if (saldoPendienteAcreditacion > 0)
        {
            decimal espacioDisponible = LIMITE_SALDO - saldo;
            decimal montoAAcreditar = Math.Min(saldoPendienteAcreditacion, espacioDisponible);

            saldo += montoAAcreditar;
            saldoPendienteAcreditacion -= montoAAcreditar;
        }
    }

    public virtual bool Descontar(decimal monto)
    {
        decimal nuevoSaldo = saldo - monto;

        // ✅ VERIFICACIÓN DE LÍMITE DE SALDO NEGATIVO
        if (nuevoSaldo < SALDO_NEGATIVO_MAXIMO)
            return false; // ❌ No permite superar el límite de -$1200

        saldo = nuevoSaldo; // ✅ Permite saldo negativo hasta -$1200

        // Después de descontar, intentar acreditar saldo pendiente
        AcreditarCarga();

        return true;
    }

    protected void RegistrarViaje()
    {
        historialViajes.Add(DateTime.Now);
    }

    protected int CantidadViajesHoy()
    {
        DateTime hoy = DateTime.Today;
        return historialViajes.Count(v => v.Date == hoy);
    }

    protected bool PuedeViajarMedioBoleto()
    {
        if (historialViajes.Count == 0) return true;

        DateTime ultimoViaje = historialViajes.Last();
        TimeSpan tiempoDesdeUltimoViaje = DateTime.Now - ultimoViaje;

        // Para testing: 5 segundos en lugar de 5 minutos
        if (tiempoDesdeUltimoViaje.TotalSeconds < 5)  // ← Cambiado de Minutes a Seconds
            return false;

        // Verificar máximo 2 viajes por día
        if (CantidadViajesHoy() >= 2)
            return false;

        return true;
    }

    protected bool PuedeViajarGratuito()
    {
        return CantidadViajesHoy() < 2;
    }

    public abstract decimal CalcularMontoPasaje(decimal tarifaBase);
    public abstract bool PuedePagar(decimal tarifaBase);
}