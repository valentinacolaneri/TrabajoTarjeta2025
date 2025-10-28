using NUnit.Framework;
using System;
using System.Threading;

[TestFixture]
public class MedioBoletoLimitesTests
{
    [Test]
    public void MedioBoleto_NoPermiteDosViajesEnMenosDe5Minutos()
    {
        var medioBoleto = new MedioBoletoEstudiantil();
        medioBoleto.Cargar(5000m);
        var colectivo = new Colectivo("132");

        // Primer viaje
        var boleto1 = colectivo.PagarCon(medioBoleto);

        // Intentar segundo viaje inmediatamente (debe fallar)
        var boleto2 = colectivo.PagarCon(medioBoleto);

        Assert.IsTrue(boleto1.EsValido);
        Assert.IsFalse(boleto2.EsValido);
        Assert.AreEqual(5000m - 790m, medioBoleto.Saldo);
    }

    [Test]
    public void MedioBoleto_PermiteDosViajesPorDia()
    {
        var medioBoleto = new MedioBoletoEstudiantil();
        medioBoleto.Cargar(5000m);
        var colectivo = new Colectivo("132");

        // Primer viaje
        var boleto1 = colectivo.PagarCon(medioBoleto);

        // Simular espera de 6 minutos (en realidad usarías mocking)
        // Thread.Sleep(100); 

        // Segundo viaje
        var boleto2 = colectivo.PagarCon(medioBoleto);

        Assert.IsTrue(boleto1.EsValido);
        Assert.IsTrue(boleto2.EsValido);
        Assert.AreEqual(5000m - 790m - 790m, medioBoleto.Saldo);
    }

    [Test]
    public void MedioBoleto_TercerViajeDelDia_TarifaCompleta()
    {
        var medioBoleto = new MedioBoletoEstudiantil();
        medioBoleto.Cargar(10000m);
        var colectivo = new Colectivo("132");

        // Primer viaje (medio boleto)
        var boleto1 = colectivo.PagarCon(medioBoleto);

        // Segundo viaje (medio boleto)
        var boleto2 = colectivo.PagarCon(medioBoleto);

        // Tercer viaje (tarifa completa)
        var boleto3 = colectivo.PagarCon(medioBoleto);

        Assert.IsTrue(boleto1.EsValido);
        Assert.IsTrue(boleto2.EsValido);
        Assert.IsTrue(boleto3.EsValido);
        Assert.AreEqual(790m, boleto1.Monto);
        Assert.AreEqual(790m, boleto2.Monto);
        Assert.AreEqual(1580m, boleto3.Monto);
    }
}
