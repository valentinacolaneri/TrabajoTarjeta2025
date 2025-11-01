using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Boleto
{
    // PROPIEDADES EXISTENTES
    public decimal Monto { get; private set; }
    public string LineaColectivo { get; private set; }
    public DateTime FechaHora { get; private set; }
    public bool EsValido { get; private set; }

    // NUEVAS PROPIEDADES para iteración 4
    public string TipoTarjeta { get; private set; }
    public decimal SaldoRestante { get; private set; }
    public int IdTarjeta { get; private set; }
    public decimal TotalAbonado { get; private set; }
    public decimal MontoRecarga { get; private set; }
    public bool EsTrasbordo { get; private set; }
    public string MotivoInvalidez { get; private set; }

    // CONSTRUCTOR ACTUALIZADO para iteración 4
    public Boleto(decimal monto, string lineaColectivo, DateTime fechaHora, bool esValido,
                 string tipoTarjeta = "", decimal saldoRestante = 0, int idTarjeta = 0,
                 decimal totalAbonado = 0, decimal montoRecarga = 0,
                 bool esTrasbordo = false, string motivo = "")
    {
        // Propiedades existentes
        Monto = monto;
        LineaColectivo = lineaColectivo;
        FechaHora = fechaHora;
        EsValido = esValido;

        // Nuevas propiedades para iteración 4
        TipoTarjeta = tipoTarjeta;
        SaldoRestante = saldoRestante;
        IdTarjeta = idTarjeta;
        TotalAbonado = totalAbonado;
        MontoRecarga = montoRecarga;
        EsTrasbordo = esTrasbordo;
        MotivoInvalidez = motivo;
    }

    public override string ToString()
    {
        string infoBasica = $"Boleto - Línea: {LineaColectivo}, Monto: ${Monto}, " +
                           $"Fecha: {FechaHora:dd/MM/yyyy HH:mm}, Válido: {EsValido}";

        // Agregar información adicional si está disponible
        if (!string.IsNullOrEmpty(TipoTarjeta))
        {
            infoBasica += $", Tarjeta: {TipoTarjeta}";
        }
        if (EsTrasbordo)
        {
            infoBasica += $", TRASBORDO";
        }
        if (!string.IsNullOrEmpty(MotivoInvalidez))
        {
            infoBasica += $", Motivo: {MotivoInvalidez}";
        }

        return infoBasica;
    }
}