namespace SuEnvioSeguro.API.Services.Singletons
{
    public sealed class GeneradorCodigoFactura 
    {
        private static readonly GeneradorCodigoFactura _instancia = new GeneradorCodigoFactura();
        private int _contador = 1000; // Simulación de secuencia

        // Constructor privado para evitar 'new GeneradorCodigoFactura()'
        private GeneradorCodigoFactura() { }

        public static GeneradorCodigoFactura ObtenerInstancia() 
        {
            return _instancia;
        }

        public string GenerarSiguienteCodigo() 
        {
            _contador++;
            return $"FAC-{DateTime.Now.Year}-{_contador}";
        }
    }
}