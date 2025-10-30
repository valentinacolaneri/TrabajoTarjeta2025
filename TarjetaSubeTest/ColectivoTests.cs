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
    public class ColectivoTests
    {
        [Test]
        public void Colectivo_Crear_ConLinea()
        {
            var colectivo = new Colectivo("132");

            // No hay propiedad pública para línea, pero podemos testear la creación
            Assert.IsNotNull(colectivo);
        }

        [Test]
        public void PagarCon_TarjetaComun_SaldoSuficiente_GeneraBoletoValido()
        {
            var colectivo = new Colectivo("132");
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(2000m);

            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto.EsValido);
            Assert.AreEqual(1580m, boleto.Monto);
        }

        [Test]
        public void PagarCon_TarjetaComun_SaldoInsuficiente_GeneraBoletoInvalido()
        {
            var colectivo = new Colectivo("132");
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(1000m);

            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsFalse(boleto.EsValido);
            Assert.AreEqual(1580m, boleto.Monto);
        }

        [Test]
        public void PagarCon_MedioBoleto_GeneraBoletoConMitadDeTarifa()
        {
            var colectivo = new Colectivo("143");
            var tarjeta = new MedioBoletoEstudiantil();
            tarjeta.Cargar(2000m);

            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto.EsValido);
            Assert.AreEqual(790m, boleto.Monto); // 1580 / 2 = 790
        }

        [Test]
        public void PagarCon_FranquiciaCompleta_GeneraBoletoGratuito()
        {
            var colectivo = new Colectivo("K");
            var tarjeta = new FranquiciaCompleta();

            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto.EsValido);
            Assert.AreEqual(0m, boleto.Monto);
        }

        [Test]
        public void PagarCon_BoletoGratuito_GeneraBoletoGratuito()
        {
            var colectivo = new Colectivo("115");
            var tarjeta = new BoletoGratuitoEstudiantil();

            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto.EsValido);
            Assert.AreEqual(0m, boleto.Monto);
        }

        [Test]
        public void PagarCon_MultipleVeces_ActualizaSaldoCorrectamente()
        {
            var colectivo = new Colectivo("132");
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(5000m);

            var boleto1 = colectivo.PagarCon(tarjeta);
            var boleto2 = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto1.EsValido);
            Assert.IsTrue(boleto2.EsValido);
            Assert.AreEqual(5000m - 1580m - 1580m, tarjeta.Saldo);
        }

        [Test]
        public void PagarCon_SaldoNegativo_DentroDelLimite_PermiteViaje()
        {
            var colectivo = new Colectivo("132");
            var tarjeta = new TarjetaComun();
            tarjeta.Cargar(2000m);

            // Primer viaje
            colectivo.PagarCon(tarjeta);
            // Segundo viaje (queda saldo negativo)
            var boleto = colectivo.PagarCon(tarjeta);

            Assert.IsTrue(boleto.EsValido);
            Assert.AreEqual(2000m - 1580m - 1580m, tarjeta.Saldo); // -1160
        }
    }
}
