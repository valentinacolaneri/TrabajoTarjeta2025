using TarjetaSube;
using NUnit.Framework;

namespace TarjetaSubeTest
{
    public class Tests
    {

        Tarjeta t;

        [SetUp]
        public void Setup()
        {
            t = new TarjetaComun();
        }

        [Test]
        public void CargaTest()
        {
            t.Cargar(100);
            t.Descontar(12);
            Assert.AreEqual(t.Saldo, 50);
        }
    }
}