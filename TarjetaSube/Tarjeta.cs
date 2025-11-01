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
        private const decimal LIMITE_SALDO = 56000m;
        private const decimal SALDO_NEGATIVO_MAXIMO = -1200m;
        protected static readonly decimal[] CARGAS_ACEPTADAS =
            { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

        // === NUEVO: Contador para IDs automáticos ===
        private static int proximoId = 1;

        // === PROPIEDADES ===
        public decimal Saldo => saldo;
        public int Id { get; protected set; }  // NUEVA PROPIEDAD

        // === PROPIEDADES PARA ITERACIÓN 4 ===
        protected BoletoFrecuente boletoFrecuente;
        public BoletoFrecuente BoletoFrecuente => boletoFrecuente;
        protected DateTime? ultimoViaje = null;
        protected string ultimaLinea = null;

        protected Tarjeta()
        {
            saldo = 0m;
            Id = proximoId++;  // NUEVO: Asignar ID automático
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

        // === NUEVOS MÉTODOS PARA ITERACIÓN 4 ===

        // Para franjas horarias
        public virtual bool EstaEnFranjaHorariaPermitida()
        {
            // Para tarjetas comunes, siempre permitido
            return true;
        }

        // Para trasbordos
        public bool PuedeRealizarTrasbordo(string lineaActual)
        {
            if (ultimoViaje == null || ultimaLinea == null)
                return false;

            // No puede ser trasbordo si es la misma línea
            if (lineaActual == ultimaLinea)
                return false;

            TimeSpan tiempoDesdeUltimoViaje = DateTime.Now - ultimoViaje.Value;

            // Verificar franja horaria de trasbordos (Lunes a Sábado 7:00-22:00)
            DateTime ahora = DateTime.Now;
            bool enFranjaTrasbordo = (ahora.DayOfWeek >= DayOfWeek.Monday && ahora.DayOfWeek <= DayOfWeek.Saturday) &&
                                    (ahora.Hour >= 7 && ahora.Hour < 22);

            return tiempoDesdeUltimoViaje.TotalMinutes <= 60 && enFranjaTrasbordo;
        }

        // Para registrar viajes (trasbordos)
        public void RegistrarViajeConLinea(string linea)
        {
            ultimoViaje = DateTime.Now;
            ultimaLinea = linea;
        }
    }
}