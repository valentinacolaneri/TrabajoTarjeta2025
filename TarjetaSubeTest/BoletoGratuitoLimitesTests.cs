using NUnit.Framework;

[TestFixture]
public class BoletoGratuitoLimitesTests
{
    [Test]
    public void BoletoGratuito_PermiteDosViajesGratuitosPorDia()
    {
        var tarjetaGratuita = new BoletoGratuitoEstudiantil();
        var colectivo = new Colectivo("132");

        // Primer viaje gratuito
        var boleto1 = colectivo.PagarCon(tarjetaGratuita);

        // Segundo viaje gratuito
        var boleto2 = colectivo.PagarCon(tarjetaGratuita);

        Assert.IsTrue(boleto1.EsValido);
        Assert.IsTrue(boleto2.EsValido);
        Assert.AreEqual(0m, boleto1.Monto);
        Assert.AreEqual(0m, boleto2.Monto);
    }

    [Test]
    public void BoletoGratuito_TercerViajeDelDia_TarifaCompleta()
    {
        var tarjetaGratuita = new BoletoGratuitoEstudiantil();
        tarjetaGratuita.Cargar(5000m);
        var colectivo = new Colectivo("132");

        // Primer viaje gratuito
        var boleto1 = colectivo.PagarCon(tarjetaGratuita);

        // Segundo viaje gratuito
        var boleto2 = colectivo.PagarCon(tarjetaGratuita);

        // Tercer viaje (tarifa completa)
        var boleto3 = colectivo.PagarCon(tarjetaGratuita);

        Assert.IsTrue(boleto1.EsValido);
        Assert.IsTrue(boleto2.EsValido);
        Assert.IsTrue(boleto3.EsValido);
        Assert.AreEqual(0m, boleto1.Monto);
        Assert.AreEqual(0m, boleto2.Monto);
        Assert.AreEqual(1580m, boleto3.Monto);
        Assert.AreEqual(5000m - 1580m, tarjetaGratuita.Saldo);
    }
}
