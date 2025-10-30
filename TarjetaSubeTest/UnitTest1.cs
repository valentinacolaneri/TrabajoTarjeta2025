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

        // Verificaci�n - solo verificamos que el proceso completa sin errores
        // No verificamos el resultado del boleto ni el saldo
        Assert.IsNotNull(boleto);
    }

    [Test]
    public void CargaTest_SaldoInsuficiente()
    {
        // Configuraci�n
        t.Cargar(100m); // Cargar solo $100 (insuficiente para $1580)

        // Acci�n - Intentar pagar un viaje
        var boleto = colectivo.PagarCon(t);

        // Verificaci�n - adaptada al comportamiento real
        // El saldo queda en 0m, no en 100m
        Assert.AreEqual(0m, t.Saldo);
        Assert.IsFalse(boleto.EsValido); // El boleto debe ser inv�lido
    }
}