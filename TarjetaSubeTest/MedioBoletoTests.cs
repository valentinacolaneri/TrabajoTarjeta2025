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
        public void MedioBoleto_CalculaMitadDeTarifa()
        {
            var tarjeta = new MedioBoletoEstudiantil();

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

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void MedioBoleto_PuedePagar_ConSaldoInsuficiente()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(500m);

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsFalse(puedePagar);
        }

        [Test]
        public void MedioBoleto_Descontar_MitadDeTarifa()
        {
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(2000m);

            bool resultado = tarjeta.Descontar(tarjeta.CalcularMontoPasaje(1580m));

            Assert.IsTrue(resultado);
            Assert.AreEqual(1210m, tarjeta.Saldo); // 2000 - 790 = 1210
        }

        [Test]
        public void MedioBoleto_Cargar_MontoAceptado()
        {
            var tarjeta = new MedioBoletoEstudiantil();

            bool resultado = tarjeta.Cargar(5000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(5000m, tarjeta.Saldo);
        }
    }
}
