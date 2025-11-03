using NUnit.Framework;
using TarjetaSube;
using System;
using System.Linq;
using System.Reflection;

namespace TarjetaSube.Tests
{
    public class TarjetaDummy : Tarjeta
    {
        public override decimal CalcularMontoPasaje(decimal tarifaBase) => tarifaBase;
        public override bool PuedePagar(decimal tarifaBase) => saldo >= tarifaBase;
        public void SetSaldo(decimal valor) => saldo = valor;
        public void SetSaldoPendiente(decimal valor) => saldoPendienteAcreditacion = valor;

        // Nos da acceso controlado a los viajes para probar
        public void RegistrarViajeManual(DateTime fecha)
        {
            var campoHistorial = typeof(Tarjeta)
                .GetField("historialViajes", BindingFlags.NonPublic | BindingFlags.Instance);
            var lista = (System.Collections.Generic.List<DateTime>)campoHistorial.GetValue(this);
            lista.Add(fecha);

            var campoMensual = typeof(Tarjeta)
                .GetField("historialViajesMensual", BindingFlags.NonPublic | BindingFlags.Instance);
            var listaMensual = (System.Collections.Generic.List<DateTime>)campoMensual.GetValue(this);
            listaMensual.Add(fecha);
        }

        public void LimpiarViajes()
        {
            var campoHistorial = typeof(Tarjeta)
                .GetField("historialViajes", BindingFlags.NonPublic | BindingFlags.Instance);
            ((System.Collections.Generic.List<DateTime>)campoHistorial.GetValue(this)).Clear();

            var campoMensual = typeof(Tarjeta)
                .GetField("historialViajesMensual", BindingFlags.NonPublic | BindingFlags.Instance);
            ((System.Collections.Generic.List<DateTime>)campoHistorial.GetValue(this)).Clear();
        }
    }

    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        public void Cargar_DeudaCompletaYSaldoDentroDelLimite_ActualizaSaldoCorrectamente()
        {
            // Arrange
            var tarjeta = new TarjetaDummy();
            tarjeta.SetSaldo(-500m); // deuda inicial
            decimal montoCarga = 2000m;

            // Act
            bool resultado = tarjeta.Cargar(montoCarga);

            // Assert
            Assert.IsTrue(resultado, "La carga debería ser exitosa.");
            // Paga la deuda (500) y el resto (1500) queda como saldo
            Assert.AreEqual(1500m, tarjeta.Saldo, "El saldo debería reflejar el monto restante después de pagar la deuda.");
            Assert.AreEqual(0m, tarjeta.SaldoPendienteAcreditacion, "No debería haber saldo pendiente de acreditación.");
        }

        [Test]
        public void Cargar_DeudaCompletaPeroExcedeLimite_SaldoAlLimiteYPendienteCorrecto()
        {
            // Arrange
            var tarjeta = new TarjetaDummy();
            tarjeta.SetSaldo(-1000m); // deuda inicial
            decimal montoCarga = 60000m; // monto que excede el límite

            // Act
            bool resultado = tarjeta.Cargar(montoCarga);

            // Assert
            Assert.IsTrue(resultado, "La carga debería ser exitosa.");
            Assert.AreEqual(56000m, tarjeta.Saldo, "El saldo no debería superar el límite establecido (56000).");
            decimal excedenteEsperado = 60000m - 1000m - 56000m;
            Assert.AreEqual(excedenteEsperado, tarjeta.SaldoPendienteAcreditacion, "El saldo pendiente debe coincidir con el excedente sobre el límite.");
        }

        [Test]
        public void Cargar_MontoInsuficienteParaCubrirDeuda_ReduceDeudaParcialmente()
        {
            // Arrange
            var tarjeta = new TarjetaDummy();
            tarjeta.SetSaldo(-1000m); // deuda inicial
            decimal montoCarga = 500m; // monto insuficiente

            // Act
            bool resultado = tarjeta.Cargar(montoCarga);

            // Assert
            Assert.IsTrue(resultado, "La carga debería ser exitosa incluso si no cubre toda la deuda.");
            Assert.AreEqual(-500m, tarjeta.Saldo, "El saldo debería reflejar la deuda restante (-500).");
            Assert.AreEqual(0m, tarjeta.SaldoPendienteAcreditacion, "No debería haber saldo pendiente de acreditación.");
        }

        [Test]
        public void CantidadViajesHoy_SinViajes_RetornaCero()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();

            int resultado = tarjeta.CantidadViajesHoy();

            Assert.AreEqual(0, resultado);
        }

        [Test]
        public void CantidadViajesHoy_ConViajeHoy_RetornaUno()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now);

            int resultado = tarjeta.CantidadViajesHoy();

            Assert.AreEqual(1, resultado);
        }

        [Test]
        public void PuedeViajarMedioBoleto_SinViajes_RetornaTrue()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();

            bool resultado = tarjeta.PuedeViajarMedioBoleto();

            Assert.IsTrue(resultado);
        }

        [Test]
        public void PuedeViajarMedioBoleto_ViajeReciente_RetornaFalse()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now.AddSeconds(-2)); // último viaje hace 2 segundos

            bool resultado = tarjeta.PuedeViajarMedioBoleto();

            Assert.IsFalse(resultado);
        }

        [Test]
        public void PuedeViajarMedioBoleto_MasDeCincoSegundosYMenosDeDosViajes_RetornaTrue()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now.AddSeconds(-10)); // viaje viejo

            bool resultado = tarjeta.PuedeViajarMedioBoleto();

            Assert.IsTrue(resultado);
        }

        [Test]
        public void PuedeViajarMedioBoleto_DosViajesHoy_RetornaFalse()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now.AddHours(-1));
            tarjeta.RegistrarViajeManual(DateTime.Now.AddMinutes(-10));

            bool resultado = tarjeta.PuedeViajarMedioBoleto();

            Assert.IsFalse(resultado);
        }

        [Test]
        public void PuedeViajarGratuito_MenosDeDosViajes_RetornaTrue()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now);

            bool resultado = tarjeta.PuedeViajarGratuito();

            Assert.IsTrue(resultado);
        }

        [Test]
        public void PuedeViajarGratuito_DosViajesHoy_RetornaFalse()
        {
            var tarjeta = new TarjetaDummy();
            tarjeta.LimpiarViajes();
            tarjeta.RegistrarViajeManual(DateTime.Now);
            tarjeta.RegistrarViajeManual(DateTime.Now.AddMinutes(-1));

            bool resultado = tarjeta.PuedeViajarGratuito();

            Assert.IsFalse(resultado);
        }
        [Test]
        public void AcreditarCarga_EspacioSuficiente_AcreditaTodoElSaldoPendiente()
        {
            // Arrange
            var tarjeta = new TarjetaDummy();
            tarjeta.SetSaldo(10000m);                  // Saldo actual
            tarjeta.SetSaldoPendiente(5000m);          // Pendiente menor al espacio disponible

            // Act
            tarjeta.AcreditarCarga();

            // Assert
            Assert.AreEqual(15000m, tarjeta.Saldo, "Debería haberse acreditado todo el saldo pendiente.");
            Assert.AreEqual(0m, tarjeta.SaldoPendienteAcreditacion, "No debería quedar saldo pendiente.");
        }

        [Test]
        public void AcreditarCarga_EspacioInsuficiente_AcreditaHastaElLimiteYDejaPendiente()
        {
            // Arrange
            var tarjeta = new TarjetaDummy();
            tarjeta.SetSaldo(55000m);                  // Cerca del límite (56000)
            tarjeta.SetSaldoPendiente(10000m);         // Pendiente mayor al espacio disponible (1000 disponible)

            // Act
            tarjeta.AcreditarCarga();

            // Assert
            Assert.AreEqual(56000m, tarjeta.Saldo, "Debería haberse acreditado solo hasta el límite de saldo.");
            Assert.AreEqual(9000m, tarjeta.SaldoPendienteAcreditacion, "Debería quedar pendiente el excedente.");
        }
    }
}
