using System;
using System.Threading;
using TarjetaSube;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEMA DE TRANSPORTE ROSARIO ===\n");

        // Iteración 1: Funcionalidad básica
        Console.WriteLine("--- ITERACIÓN 1: Tarjeta Común ---");
        EjemploIteracion1();

        // Iteración 2: Nuevas funcionalidades
        Console.WriteLine("\n--- ITERACIÓN 2: Diferentes Franquicias ---");
        EjemploIteracion2();

        // Iteración 3: Nuevas funcionalidades
        Console.WriteLine("\n--- ITERACIÓN 3: Límites de Medio Boleto ---");
        EjemploMedioBoletoLimites();

        Console.WriteLine("\n--- ITERACIÓN 3: Límites de Boleto Gratuito ---");
        EjemploBoletoGratuitoLimites();

        Console.WriteLine("\n--- ITERACIÓN 3: Nuevo Límite de Saldo $56000 ---");
        EjemploNuevoLimiteSaldo();

        Console.WriteLine("\n--- ITERACIÓN 3: Información Completa del Boleto ---");
        EjemploInformacionBoletos();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    static void EjemploIteracion1()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(5000m);
        Console.WriteLine($"Saldo inicial: ${tarjeta.Saldo}");

        var colectivo132 = new Colectivo("132");
        var boleto1 = colectivo132.PagarCon(tarjeta);

        Console.WriteLine($"Boleto: {boleto1}");
        Console.WriteLine($"Saldo después del viaje: ${tarjeta.Saldo}");
    }

    static void EjemploIteracion2()
    {
        var tarjetas = new Tarjeta[]
        {
            new TarjetaComun(),
            new MedioBoletoEstudiantil(),
            new BoletoGratuitoEstudiantil(),
            new FranquiciaCompleta()
        };

        var colectivo = new Colectivo("143");

        foreach (var tarjeta in tarjetas)
        {
            tarjeta.Cargar(2000m);
            var boleto = colectivo.PagarCon(tarjeta);

            string tipoTarjeta = tarjeta.GetType().Name;
            Console.WriteLine($"{tipoTarjeta}:");
            Console.WriteLine($"  Boleto: ${boleto.Monto} - Válido: {boleto.EsValido}");
            Console.WriteLine($"  Saldo restante: ${tarjeta.Saldo}\n");
        }
    }

    static void EjemploMedioBoletoLimites()
    {
        var medioBoleto = new MedioBoletoEstudiantil();
        medioBoleto.Cargar(5000m);
        var colectivo = new Colectivo("132");

        Console.WriteLine("Primer viaje con medio boleto:");
        var boleto1 = colectivo.PagarCon(medioBoleto);
        Console.WriteLine($"  Monto: ${boleto1.Monto} - Válido: {boleto1.EsValido}");
        Console.WriteLine($"  Saldo: ${medioBoleto.Saldo}");

        Console.WriteLine("\nIntentando segundo viaje inmediatamente:");
        var boleto2 = colectivo.PagarCon(medioBoleto);
        Console.WriteLine($"  Monto: ${boleto2.Monto} - Válido: {boleto2.EsValido}");
        Console.WriteLine($"  (Debe fallar - menos de 5 minutos entre viajes)");

        Console.WriteLine($"\nSaldo actual: ${medioBoleto.Saldo}");

        Console.WriteLine("\nEsperando 6 minutos...");
        // En un caso real aquí habría una espera, pero para demo mostramos el concepto
        Console.WriteLine("(En producción esperaría 5 minutos reales)");

        Console.WriteLine("\nSegundo viaje después de espera:");
        var boleto3 = colectivo.PagarCon(medioBoleto);
        Console.WriteLine($"  Monto: ${boleto3.Monto} - Válido: {boleto3.EsValido}");

        Console.WriteLine("\nTercer viaje del día (tarifa completa):");
        var boleto4 = colectivo.PagarCon(medioBoleto);
        Console.WriteLine($"  Monto: ${boleto4.Monto} - Válido: {boleto4.EsValido}");
        Console.WriteLine($"  (Debe cobrar tarifa completa - máximo 2 medios boletos/día)");
    }

    static void EjemploBoletoGratuitoLimites()
    {
        var tarjetaGratuita = new BoletoGratuitoEstudiantil();
        tarjetaGratuita.Cargar(5000m);
        var colectivo = new Colectivo("115");

        Console.WriteLine("Primer viaje gratuito:");
        var boleto1 = colectivo.PagarCon(tarjetaGratuita);
        Console.WriteLine($"  Monto: ${boleto1.Monto} - Válido: {boleto1.EsValido}");

        Console.WriteLine("\nSegundo viaje gratuito:");
        var boleto2 = colectivo.PagarCon(tarjetaGratuita);
        Console.WriteLine($"  Monto: ${boleto2.Monto} - Válido: {boleto2.EsValido}");

        Console.WriteLine("\nTercer viaje (debe cobrar tarifa completa):");
        var boleto3 = colectivo.PagarCon(tarjetaGratuita);
        Console.WriteLine($"  Monto: ${boleto3.Monto} - Válido: {boleto3.EsValido}");
        Console.WriteLine($"  Saldo restante: ${tarjetaGratuita.Saldo}");
    }

    static void EjemploNuevoLimiteSaldo()
    {
        var tarjeta = new TarjetaComun();

        Console.WriteLine("Cargando $55000...");
        tarjeta.Cargar(55000m);
        Console.WriteLine($"  Saldo: ${tarjeta.Saldo}");
        Console.WriteLine($"  Pendiente acreditación: ${tarjeta.SaldoPendienteAcreditacion}");

        Console.WriteLine("\nCargando $5000 adicionales...");
        tarjeta.Cargar(5000m);
        Console.WriteLine($"  Saldo: ${tarjeta.Saldo} (límite máximo)");
        Console.WriteLine($"  Pendiente acreditación: ${tarjeta.SaldoPendienteAcreditacion}");

        Console.WriteLine("\nRealizando viaje para liberar espacio...");
        var colectivo = new Colectivo("132");
        var boleto = colectivo.PagarCon(tarjeta);
        Console.WriteLine($"  Viaje: ${boleto.Monto}");
        Console.WriteLine($"  Nuevo saldo: ${tarjeta.Saldo}");
        Console.WriteLine($"  Nuevo pendiente: ${tarjeta.SaldoPendienteAcreditacion}");
        Console.WriteLine($"  (Se acreditaron ${boleto.Monto} automáticamente)");
    }

    static void EjemploInformacionBoletos()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(3000m);
        var colectivo = new Colectivo("K");

        Console.WriteLine("Generando boleto con información completa:");
        var boleto = colectivo.PagarCon(tarjeta);

        Console.WriteLine("\n" + boleto.ObtenerInformacionCompleta());

        Console.WriteLine("\n--- Boleto con saldo negativo ---");
        var tarjeta2 = new TarjetaComun();
        tarjeta2.Cargar(1000m);

        // Primer viaje para generar saldo negativo
        colectivo.PagarCon(tarjeta2);

        // Recargar y viajar again para mostrar recarga
        tarjeta2.Cargar(2000m);
        var boletoConRecarga = colectivo.PagarCon(tarjeta2);

        Console.WriteLine("\n" + boletoConRecarga.ObtenerInformacionCompleta());
    }

    static void EjemploSaldoNegativo()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(1000m);
        Console.WriteLine($"Saldo inicial: ${tarjeta.Saldo}");

        var colectivo = new Colectivo("132");

        for (int i = 1; i <= 3; i++)
        {
            var boleto = colectivo.PagarCon(tarjeta);
            Console.WriteLine($"Viaje {i}: ${boleto.Monto} - Válido: {boleto.EsValido} - Saldo: ${tarjeta.Saldo}");

            if (!boleto.EsValido)
            {
                Console.WriteLine("  ❌ No se pudo realizar el viaje (saldo insuficiente)");
                break;
            }
        }

        Console.WriteLine("\nRecargando $2000...");
        tarjeta.Cargar(2000m);
        Console.WriteLine($"Saldo después de recargar: ${tarjeta.Saldo}");
    }
}
