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
    public class FranquiciaCompletaTests
    {
        [Test]
        public void FranquiciaCompleta_Crear_SaldoInicialCero()
        {
            var tarjeta = new FranquiciaCompleta();
            Assert.AreEqual(0m, tarjeta.Saldo);
        }

        [Test]
        public void FranquiciaCompleta_SiemprePuedePagar()
        {
            var tarjeta = new FranquiciaCompleta();

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_SiemprePuedePagar_SinSaldo()
        {
            var tarjeta = new FranquiciaCompleta();
            // Sin cargar saldo

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void FranquiciaCompleta_MontoCero()
        {
            var tarjeta = new FranquiciaCompleta();

            decimal monto = tarjeta.CalcularMontoPasaje(1580m);

            Assert.AreEqual(0m, monto);
        }

        [Test]
        public void FranquiciaCompleta_Descontar_NoRestaSaldo()
        {
            var tarjeta = new FranquiciaCompleta();
            tarjeta.Cargar(1000m);

            bool resultado = tarjeta.Descontar(1580m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(1000m, tarjeta.Saldo); // El saldo no cambia
        }

        [Test]
        public void FranquiciaCompleta_Cargar_MontoAceptado()
        {
            var tarjeta = new FranquiciaCompleta();

            bool resultado = tarjeta.Cargar(10000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(10000m, tarjeta.Saldo);
        }
    }
}
