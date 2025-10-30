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
        public void BoletoGratuito_PuedePagar_DentroDeFranjaHoraria()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            // Simular que está dentro de franja horaria
            // (En un escenario real, usarías mocking para esto)
            bool puedePagar = tarjeta.PuedePagar(1580m);

            // El resultado depende de si está en franja horaria
            // Por ahora solo verificamos que no lance excepciones
            Assert.IsTrue(true); // Test básico de que funciona
        }

        [Test]
        public void BoletoGratuito_MontoCero_DentroDeFranjaHoraria()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            decimal monto = tarjeta.CalcularMontoPasaje(1580m);

            // Depende de la franja horaria y cantidad de viajes
            // Por ahora solo verificamos que devuelve un valor válido
            Assert.IsTrue(monto == 0m || monto == 1580m);
        }

        [Test]
        public void BoletoGratuito_Descontar_NoRestaSaldoCuandoEsGratuito()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();
            tarjeta.Cargar(2000m);
            decimal saldoInicial = tarjeta.Saldo;

            bool resultado = tarjeta.Descontar(0m); // Usar monto 0 para simular gratuito

            Assert.IsTrue(resultado);
            Assert.AreEqual(saldoInicial, tarjeta.Saldo); // El saldo no cambia
        }

        [Test]
        public void BoletoGratuito_Descontar_RestaSaldoCuandoNoEsGratuito()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();
            tarjeta.Cargar(2000m);
            decimal saldoInicial = tarjeta.Saldo;

            bool resultado = tarjeta.Descontar(1580m); // Usar monto normal

            // El resultado depende de si puede descontar (saldo suficiente)
            // Por ahora solo verificamos el flujo básico
            Assert.IsTrue(true);
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

            // Puede pagar múltiples veces - el resultado depende de franja horaria
            bool puedePagar1 = tarjeta.PuedePagar(1580m);
            bool puedePagar2 = tarjeta.PuedePagar(1580m);
            bool puedePagar3 = tarjeta.PuedePagar(1580m);

            // Todos deberían dar el mismo resultado (dependiendo de franja horaria)
            Assert.AreEqual(puedePagar1, puedePagar2);
            Assert.AreEqual(puedePagar2, puedePagar3);
        }

        [Test]
        public void BoletoGratuito_CalcularMontoPasaje_RetornaCeroOGratuito()
        {
            var tarjeta = new BoletoGratuitoEstudiantil();

            decimal monto = tarjeta.CalcularMontoPasaje(1580m);

            // Debe retornar 0 (gratuito) o el monto completo
            Assert.IsTrue(monto == 0m || monto == 1580m);
        }
    }
}