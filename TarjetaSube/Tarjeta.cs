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

        decimal espacioDisponible = LIMITE_SALDO - saldo;

        if (monto <= espacioDisponible)
        {
            // Cabe todo en el saldo
            saldo += monto;
            return true;
        }
        else
        {
            // Llena hasta el límite y guarda el resto como pendiente
            saldo = LIMITE_SALDO;
            saldoPendienteAcreditacion += (monto - espacioDisponible);
            return true;
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

        if (nuevoSaldo < SALDO_NEGATIVO_MAXIMO)
            return false;

        saldo = nuevoSaldo;

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

        // Verificar 5 minutos entre viajes
        if (tiempoDesdeUltimoViaje.TotalMinutes < 5)
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