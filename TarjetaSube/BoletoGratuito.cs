using System;

namespace TarjetaSube
{
    public class BoletoGratuito
    {
        private readonly Func<DateTime> _now;
        private DateTime? _ultimoViaje;
        private int _viajesHoy;
        private DateTime _fechaUltimoConteo;
        private const double MinSegundosEntreViajes = 5.0;
        private const int MaxViajesGratuitosPorDia = 2;

        // Constructor por defecto para producción (usa UTC para consistencia)
        public BoletoGratuito() : this(() => DateTime.UtcNow) { }

        // Constructor para inyección de tiempo (útil para tests)
        public BoletoGratuito(Func<DateTime> nowProvider)
        {
            _now = nowProvider ?? (() => DateTime.UtcNow);
            _viajesHoy = 0;
            _fechaUltimoConteo = _now().Date;
        }

        public bool PermiteViaje()
        {
            var ahora = _now();

            // Reiniciar conteo si cambió el día
            if (ahora.Date != _fechaUltimoConteo)
            {
                _viajesHoy = 0;
                _fechaUltimoConteo = ahora.Date;
                _ultimoViaje = null;
            }

            // Verificar intervalo mínimo entre viajes
            if (_ultimoViaje.HasValue)
            {
                var segundos = (ahora - _ultimoViaje.Value).TotalSeconds;
                if (segundos < MinSegundosEntreViajes)
                {
                    // Demasiado pronto
                    return false;
                }
            }

            // Verificar cantidad de viajes gratuitos por día
            if (_viajesHoy < MaxViajesGratuitosPorDia)
            {
                _viajesHoy++;
                _ultimoViaje = ahora;
                return true; // viaje permitido como gratuito
            }

            // ya excedió cantidad de viajes gratuitos del día
            _ultimoViaje = ahora;
            return false;
        }

        // Método auxiliar (opcional) para tests o uso externo: reiniciar estado
        public void ReiniciarEstadoParaPruebas()
        {
            _viajesHoy = 0;
            _ultimoViaje = null;
            _fechaUltimoConteo = _now().Date;
        }
    }
}