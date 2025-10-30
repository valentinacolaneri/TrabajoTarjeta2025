﻿using System;
using System.Collections.Generic;
using System.Linq;
using TarjetaSube;

public class BoletoGratuitoEstudiantil : Tarjeta
{
    public override decimal CalcularMontoPasaje(decimal tarifaBase)
    {
        if (!EstaDentroDeFranjaHoraria())
            return tarifaBase;

        if (PuedeViajarGratuito())
        {
            return 0m; // ✅ Boleto gratuito
        }
        else
        {
            return tarifaBase; // ✅ Tarifa completa después del 2do viaje
        }
    }

    public override bool PuedePagar(decimal tarifaBase)
    {
        if (!EstaDentroDeFranjaHoraria())
            return false;

        return true;
    }

    private bool EstaDentroDeFranjaHoraria()
    {
        DateTime ahora = DateTime.Now;
        DayOfWeek dia = ahora.DayOfWeek;
        int hora = ahora.Hour;

        if (dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday)
        {
            return hora >= 6 && hora < 22;
        }

        return false;
    }

    // ✅ CORREGIDO: NO DESCONTAR NADA cuando el boleto es gratuito
    public new bool Descontar(decimal monto)
    {
        // Si el monto es 0 (boleto gratuito), NO descontar nada del saldo
        if (monto == 0)
        {
            // ✅ Solo registrar el viaje sin afectar el saldo
            RegistrarViaje();
            return true;
        }
        else
        {
            // ✅ Solo para viajes NO gratuitos (fuera de franja horaria o después del 2do viaje)
            bool resultado = base.Descontar(monto);
            if (resultado)
            {
                RegistrarViaje();
            }
            return resultado;
        }
    }
}