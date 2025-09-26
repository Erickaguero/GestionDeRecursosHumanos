using System.Security.Cryptography;

namespace PrototipoFuncionalRecursosHumanos.Services
{
    public class GeneradorContrasena
    {
        public GeneradorContrasena()
        {
        }
        public string GenerarContrasenaSegura()
        {
            int longitud = 8;
            string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#_";
            char[] chars = new char[longitud];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                for (int i = 0; i < longitud; i++)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    chars[i] = caracteresPermitidos[(int)(num % (uint)caracteresPermitidos.Length)];
                }
            }

            return new string(chars);
        }
    }
}
