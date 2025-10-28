using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSubeTest
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using TarjetaSube;

    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        public void TarjetaComun_Crear_SaldoInicialCero()
        {
            var tarjeta = new TarjetaComun();
            Assert.AreEqual(0m, tarjeta.Saldo);
        }

        [Test]
        public void Cargar_MontoAceptado_ActualizaSaldo()
        {
            var tarjeta = new TarjetaComun();
            decimal monto = 2000m;

            bool resultado = tarjeta.Cargar(monto);

            Assert.IsTrue(resultado);
            Assert.AreEqual(monto, tarjeta.Saldo);
        }

        [Test]
        public void Cargar_MontoNoAceptado_NoActualizaSaldo()
        {
            var tarjeta = new TarjetaComun();
            decimal monto = 1000m;

            bool resultado = tarjeta.Cargar(monto);

            Assert.IsFalse(resultado);
            Assert.AreEqual(0m, tarjeta.Saldo);
        }

        [Test]
        public void Cargar_ExcedeLimite_NoPermiteCarga()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(30000m);

            bool resultado = tarjeta.Cargar(20000m);

            Assert.IsFalse(resultado);
            Assert.AreEqual(30000m, tarjeta.Saldo);
        }

        [Test]
        public void Descontar_SaldoSuficiente_ActualizaSaldo()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(2000m);

            bool resultado = tarjeta.Descontar(1580m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(420m, tarjeta.Saldo);
        }

        [Test]
        public void Descontar_SaldoInsuficiente_NoActualizaSaldo()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);

            bool resultado = tarjeta.Descontar(1580m);

            Assert.IsFalse(resultado);
            Assert.AreEqual(1000m, tarjeta.Saldo);
        }

        [Test]
        public void TestTodosLosMontosAceptados()
        {
            decimal[] montosAceptados = { 2000m, 3000m, 4000m, 5000m, 8000m, 10000m, 15000m, 20000m, 25000m, 30000m };

            foreach (var monto in montosAceptados)
            {
                var tarjeta = new TarjetaComun();
                bool resultado = tarjeta.Cargar(monto);

                Assert.IsTrue(resultado, $"El monto {monto} debería ser aceptado");
                Assert.AreEqual(monto, tarjeta.Saldo);
            }
        }

        // Tests Iteración 2 - Saldo negativo
        [Test]
        public void Descontar_PermiteSaldoNegativoHastaLimite()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);

            bool resultado = tarjeta.Descontar(2000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(-1000m, tarjeta.Saldo);
        }

        [Test]
        public void Descontar_NoPermiteSaldoNegativoMasAllaDelLimite()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);

            bool resultado = tarjeta.Descontar(2500m);

            Assert.IsFalse(resultado);
            Assert.AreEqual(1000m, tarjeta.Saldo);
        }

        [Test]
        public void Cargar_ConSaldoNegativo_DescuestaDeudaPrimero()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);
            tarjeta.Descontar(1500m);

            bool resultado = tarjeta.Cargar(2000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(1500m, tarjeta.Saldo);
        }

        [Test]
        public void TarjetaComun_CalcularMontoPasaje_RetornaTarifaCompleta()
        {
            var tarjeta = new TarjetaComun();
            decimal monto = tarjeta.CalcularMontoPasaje(1580m);

            Assert.AreEqual(1580m, monto);
        }

        [Test]
        public void TarjetaComun_PuedePagar_ConSaldoSuficiente()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(2000m);

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void TarjetaComun_PuedePagar_ConSaldoInsuficiente()
        {
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsFalse(puedePagar);
        }
    }
    }