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

        public void SincronizarConsecutivo(int ultimoConsecutivoRegistrado)
        {
            while (true)
            {
                var consecutivoActual = Volatile.Read(ref _contador);

                if (consecutivoActual >= ultimoConsecutivoRegistrado)
                {
                    return;
                }

                if (Interlocked.CompareExchange(ref _contador, ultimoConsecutivoRegistrado, consecutivoActual) == consecutivoActual)
                {
                    return;
                }
            }
        }

        public string GenerarSiguienteCodigo() 
        {
            var consecutivo = Interlocked.Increment(ref _contador);
            return $"FAC-{DateTime.UtcNow:yyyyMMdd}-{consecutivo:D6}";
        }
    }
}