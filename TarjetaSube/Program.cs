using System;
using System.Collections.Generic;
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

        // Ejemplos de saldo negativo
        Console.WriteLine("\n--- ITERACIÓN 2: Saldo Negativo ---");
        EjemploSaldoNegativo();

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    static void EjemploIteracion1()
    {
        // Crear tarjeta común y cargar saldo
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(5000m);
        Console.WriteLine($"Saldo inicial: ${tarjeta.Saldo}");

        // Viajar en colectivo
        var colectivo132 = new Colectivo("132");
        var boleto1 = colectivo132.PagarCon(tarjeta);

        Console.WriteLine($"Boleto: {boleto1}");
        Console.WriteLine($"Saldo después del viaje: ${tarjeta.Saldo}");
    }

    static void EjemploIteracion2()
    {
        // Probar diferentes tipos de tarjetas
        var tarjetas = new List<Tarjeta>
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

    static void EjemploSaldoNegativo()
    {
        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(1000m);
        Console.WriteLine($"Saldo inicial: ${tarjeta.Saldo}");

        var colectivo = new Colectivo("K");

        // Intentar viajes que generen saldo negativo
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

        // Recargar y ver cómo se descuenta la deuda
        Console.WriteLine("\nRecargando $2000...");
        tarjeta.Cargar(2000m);
        Console.WriteLine($"Saldo después de recargar: ${tarjeta.Saldo}");
    }

    static void EjemploCargasAceptadas()
    {
        Console.WriteLine("\n--- TESTEO DE CARGAS ACEPTADAS ---");
        var tarjeta = new TarjetaComun();

        decimal[] montosPrueba = { 1000m, 2000m, 5000m, 25000m, 35000m };

        foreach (var monto in montosPrueba)
        {
            bool exito = tarjeta.Cargar(monto);
            string resultado = exito ? "✅ Aceptada" : "❌ Rechazada";
            Console.WriteLine($"Carga de ${monto}: {resultado} - Saldo: ${tarjeta.Saldo}");
        }
    }
}
