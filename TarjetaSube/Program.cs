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
        Console.WriteLine("\n=== ITERACIÓN 2: SISTEMA DE SALDO NEGATIVO ===");
        EjemploSaldoNegativo();

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
        Console.WriteLine("🚌 DEMOSTRACIÓN DEL SISTEMA DE SALDO NEGATIVO (-$1200)");
        Console.WriteLine("=====================================================");

        var tarjeta = new TarjetaComun();
        tarjeta.Cargar(1000m);
        Console.WriteLine($"\n1. Saldo inicial después de cargar $1000: ${tarjeta.Saldo}");

        var colectivo = new Colectivo("132");

        Console.WriteLine($"\n2. Realizando viajes...");

        // Realizar varios viajes hasta llegar al límite negativo
        for (int i = 1; i <= 4; i++)
        {
            var boleto = colectivo.PagarCon(tarjeta);
            Console.WriteLine($"   Viaje {i}: ${boleto.Monto} - Saldo: ${tarjeta.Saldo} - Válido: {boleto.EsValido}");

            if (!boleto.EsValido)
            {
                Console.WriteLine("   ❌ Viaje rechazado - Límite de saldo negativo alcanzado");
                break;
            }
        }

        Console.WriteLine($"\n3. Estado final: Saldo = ${tarjeta.Saldo}");
        Console.WriteLine("   ✅ Permite saldo negativo hasta -$1200");

        // Demostrar que al recargar se paga la deuda primero
        Console.WriteLine($"\n4. Recargando $2000 para pagar la deuda...");
        tarjeta.Cargar(2000m);
        Console.WriteLine($"   Nuevo saldo después de recargar: ${tarjeta.Saldo}");
        Console.WriteLine("   ✅ La recarga primero paga la deuda, luego aumenta el saldo positivo");

        // Mostrar que funciona con diferentes montos de recarga
        Console.WriteLine($"\n5. Gastando hasta tener deuda nuevamente...");
        colectivo.PagarCon(tarjeta);
        colectivo.PagarCon(tarjeta);
        Console.WriteLine($"   Saldo después de 2 viajes: ${tarjeta.Saldo}");

        Console.WriteLine($"\n6. Recargando $500 (no alcanza para pagar toda la deuda)...");
        tarjeta.Cargar(500m);
        Console.WriteLine($"   Saldo después de recarga parcial: ${tarjeta.Saldo}");
        Console.WriteLine("   ✅ Reduce la deuda parcialmente sin llegar a saldo positivo");
    }
}
