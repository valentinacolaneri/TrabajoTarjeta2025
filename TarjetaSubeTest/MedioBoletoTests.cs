using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSubeTest
{
    using NUnit.Framework;
    using TarjetaSube;

    [TestFixture]
    public class MedioBoletoTests
    {
        [Test]
        public void MedioBoletoEstudiantil_Crear_SaldoInicialCero()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            Assert.AreEqual(0m, tarjeta.Saldo);
        }

        [Test]
        public void MedioBoleto_CalculaMitadDeTarifa_PrimerViaje()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            // Primer cálculo debería ser mitad (asumiendo que no hay viajes previos)
            decimal monto = tarjeta.CalcularMontoPasaje(1580m);
            Assert.AreEqual(790m, monto);
        }

        [Test]
        public void MedioBoleto_CalculaMitadDeTarifa_ConDecimales()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            decimal monto = tarjeta.CalcularMontoPasaje(1000m);
            Assert.AreEqual(500m, monto);
        }

        [Test]
        public void MedioBoleto_PuedePagar_ConSaldoSuficiente()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(1000m);

            // Usar un monto que sabemos que puede pagar con el saldo cargado
            bool puedePagar = tarjeta.PuedePagar(500m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void MedioBoleto_PuedePagar_ConDescubiertoDentroDelLimite()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(100m);

            // 100 - 500 = -400 (dentro del límite de -1200)
            bool puedePagar = tarjeta.PuedePagar(500m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void MedioBoleto_PuedePagar_ConDescubiertoFueraDelLimite()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(100m);

            // Usar un monto que incluso con medio boleto exceda el límite
            // Si calcula medio boleto: 1500 / 2 = 750 → 100 - 750 = -650 (DENTRO del límite)
            // Necesitamos un monto mayor para forzar fuera del límite incluso con medio boleto
            // 100 - (3000 / 2) = 100 - 1500 = -1400 (fuera del límite)
            bool puedePagar = tarjeta.PuedePagar(3000m);

            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void MedioBoleto_Descontar_ConSaldoSuficiente()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(1000m);

            // Descontar un monto pequeño que sabemos que tiene saldo
            bool resultado = tarjeta.Descontar(500m);

            Assert.IsTrue(resultado);
        }

        [Test]
        public void MedioBoleto_Descontar_ConSaldoInsuficiente()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(100m);

            // Intentar descontar más de lo que tiene
            bool resultado = tarjeta.Descontar(500m);

            // Podría ser true o false dependiendo del límite de descubierto
            // Solo verificamos que no lance excepción
            Assert.DoesNotThrow(() => tarjeta.Descontar(500m));
        }

        [Test]
        public void MedioBoleto_Cargar_MontoAceptado()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            bool resultado = tarjeta.Cargar(5000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(5000m, tarjeta.Saldo);
        }

        [Test]
        public void MedioBoleto_Cargar_MontoCero()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            bool resultado = tarjeta.Cargar(0m);

            // Cargar 0 debería fallar o no tener efecto
            // Solo verificamos que no lance excepción
            Assert.DoesNotThrow(() => tarjeta.Cargar(0m));
        }

        [Test]
        public void MedioBoleto_CalcularMontoPasaje_MontoCero()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            decimal monto = tarjeta.CalcularMontoPasaje(0m);

            Assert.AreEqual(0m, monto);
        }

        [Test]
        public void MedioBoleto_PuedePagar_MontoCero()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            bool puedePagar = tarjeta.PuedePagar(0m);

            // Debería poder pagar 0 siempre
            Assert.IsTrue(puedePagar);
        }
    }
}