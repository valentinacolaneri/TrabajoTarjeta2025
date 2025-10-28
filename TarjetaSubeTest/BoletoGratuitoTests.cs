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
    public class BoletoGratuitoTests
    {
        [Test]
        public void BoletoGratuito_Crear_SaldoInicialCero()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();
            Assert.AreEqual(0m, tarjeta.Saldo);
        }

        [Test]
        public void BoletoGratuito_SiemprePuedePagar()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void BoletoGratuito_SiemprePuedePagar_SinSaldo()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();
            // Sin cargar saldo

            bool puedePagar = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar);
        }

        [Test]
        public void BoletoGratuito_MontoCero()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            decimal monto = tarjeta.CalcularMontoPasaje(1580m);

            Assert.AreEqual(0m, monto);
        }

        [Test]
        public void BoletoGratuito_Descontar_NoRestaSaldo()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();
            tarjeta.Cargar(1000m);

            bool resultado = tarjeta.Descontar(1580m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(1000m, tarjeta.Saldo); // El saldo no cambia
        }

        [Test]
        public void BoletoGratuito_Cargar_MontoAceptado()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            bool resultado = tarjeta.Cargar(8000m);

            Assert.IsTrue(resultado);
            Assert.AreEqual(8000m, tarjeta.Saldo);
        }

        [Test]
        public void BoletoGratuito_PuedePagar_MultipleVeces()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            // Puede pagar múltiples veces sin saldo
            bool puedePagar1 = tarjeta.PuedePagar(1580m);
            bool puedePagar2 = tarjeta.PuedePagar(1580m);
            bool puedePagar3 = tarjeta.PuedePagar(1580m);

            Assert.IsTrue(puedePagar1);
            Assert.IsTrue(puedePagar2);
            Assert.IsTrue(puedePagar3);
        }
    }
}
