using System;
using System.Security.Cryptography;
using System.Text;


namespace Api_Correos.Util
{
    /// <summary>
    /// Clase con métodos para generar una llave aleatoria.
    /// </summary>
    public static class KeyGenerator
    {
        internal static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        /// <summary>
        /// Método que genera una llave en formato string a partir de un largo dado.
        /// </summary>
        /// <param name="size">Tamaño de la llave.</param>
        /// <returns>La llave en formato string.</returns>
        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Método alternativo que genera una llave en formato string a partir de un largo dado.
        /// </summary>
        /// <param name="size">Tamaño de la llave.</param>
        /// <returns>La llave en formato string.</returns>
        public static string GetUniqueKeyOriginal_BIASED(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


    }
}
