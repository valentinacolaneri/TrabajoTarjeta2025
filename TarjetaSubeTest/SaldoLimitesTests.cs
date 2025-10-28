using NUnit.Framework;

[TestFixture]
public class SaldoLimitesTests
{
    [Test]
    public void Cargar_ExcedeLimite56000_GuardaExcedenteComoPendiente()
    {
        var tarjeta = new TarjetaComun();

        // Llenar casi hasta el límite
        tarjeta.Cargar(55000m);

        // Cargar 5000 más (solo deberían entrar 1000)
        bool resultado = tarjeta.Cargar(5000m);

        Assert.IsTrue(resultado);
        Assert.AreEqual(56000m, tarjeta.Saldo);
        Assert.AreEqual(4000m, tarjeta.SaldoPendienteAcreditacion);
    }

    [Test]
    public void AcreditarCarga_DespuesDeViaje_AcreditaSaldoPendiente()
    {
        var tarjeta = new TarjetaComun();

        // Llenar hasta el límite con saldo pendiente
        tarjeta.Cargar(55000m);
        tarjeta.Cargar(5000m);

        Assert.AreEqual(56000m, tarjeta.Saldo);
        Assert.AreEqual(4000m, tarjeta.SaldoPendienteAcreditacion);

        // Realizar un viaje para liberar espacio
        var colectivo = new Colectivo("132");
        colectivo.PagarCon(tarjeta);

        // Después del viaje, debería acreditarse parte del saldo pendiente
        Assert.AreEqual(2420m, tarjeta.SaldoPendienteAcreditacion);
        Assert.AreEqual(56000m, tarjeta.Saldo);
    }
}
