using NUnit.Framework;

[TestFixture]
public class BoletoInformacionTests
{
    [Test]
    public void Boleto_ContieneInformacionCompleta()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(5000m);
        var colectivo = new Colectivo("132");

        var boleto = colectivo.PagarCon(tarjeta);

        Assert.AreEqual(1580m, boleto.Monto);
        Assert.AreEqual("132", boleto.LineaColectivo);
        Assert.AreEqual("TarjetaComun", boleto.TipoTarjeta);
        Assert.AreEqual(5000m - 1580m, boleto.SaldoRestante);
        Assert.IsTrue(boleto.IdTarjeta > 0);
        Assert.IsTrue(boleto.EsValido);
    }

    [Test]
    public void Boleto_ObtenerInformacionCompleta_FormatoCorrecto()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(5000m);
        var colectivo = new Colectivo("132");

        var boleto = colectivo.PagarCon(tarjeta);
        string info = boleto.ObtenerInformacionCompleta();

        Assert.IsTrue(info.Contains("Línea: 132"));
        Assert.IsTrue(info.Contains("Tipo Tarjeta: TarjetaComun"));
        Assert.IsTrue(info.Contains("Monto Viaje: $1580"));
    }
}
