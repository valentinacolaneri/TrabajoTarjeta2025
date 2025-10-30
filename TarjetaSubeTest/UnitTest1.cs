using NUnit.Framework;
using TarjetaSube;

[TestFixture]
public class UniTest1
{
    private TarjetaComun t;
    private Colectivo colectivo;

    [SetUp]
    public void Setup()
    {
        t = new TarjetaComun();
        colectivo = new Colectivo("132");
    }

    [Test]
    public void CargaTest()
    {
        // Configuraci�n
        t.Cargar(1000m); // Cargar $1000

        // Acci�n - Pagar un viaje
        var boleto = colectivo.PagarCon(t);

        // Verificaci�n
        Assert.AreEqual(1000m - 1580m, t.Saldo); // $1000 - $1580 = -$580
        Assert.IsTrue(boleto.EsValido); // Debe ser v�lido por el saldo negativo permitido
    }

    [Test]
    public void CargaTest_SaldoInsuficiente()
    {
        // Configuraci�n
        t.Cargar(100m); // Cargar solo $100 (insuficiente para $1580)

        // Acci�n - Intentar pagar un viaje
        var boleto = colectivo.PagarCon(t);

        // Verificaci�n
        Assert.AreEqual(100m, t.Saldo); // El saldo no debe cambiar
        Assert.IsFalse(boleto.EsValido); // El boleto debe ser inv�lido
    }
}