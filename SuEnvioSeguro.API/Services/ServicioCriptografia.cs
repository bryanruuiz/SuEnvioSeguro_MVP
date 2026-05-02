namespace SuEnvioSeguro.API.Services
{
    public class ServicioCriptografia 
    {
        // Usa BCrypt o un algoritmo seguro para hashear
        public string HashearContrasena(string passwordPlana) 
        {
            return BCrypt.Net.BCrypt.HashPassword(passwordPlana);
        }

        public bool VerificarContrasena(string hash, string passwordPlana) 
        {
            return BCrypt.Net.BCrypt.Verify(passwordPlana, hash);
        }
    }
}