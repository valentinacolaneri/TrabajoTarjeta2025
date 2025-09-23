using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Boleto
{
    public decimal Monto { get; }
    public string LineaColectivo { get; }
    public DateTime FechaHora { get; }
    public bool EsValido { get; }

    public Boleto(decimal monto, string lineaColectivo, DateTime fechaHora, bool esValido)
    {
        Monto = monto;
        LineaColectivo = lineaColectivo;
        FechaHora = fechaHora;
        EsValido = esValido;
    }

    public override string ToString()
    {
        return $"Boleto - Línea: {LineaColectivo}, Monto: ${Monto}, " +
               $"Fecha: {FechaHora:dd/MM/yyyy HH:mm}, Válido: {EsValido}";
    }
}
