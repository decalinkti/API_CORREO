using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Api_Correos.Util
{
    /// <summary>
    /// Clase para manejar la encriptación y desencriptación de datos en AES
    /// </summary>
    public class AESEncrytDecry : IAESEncrytDecry
    {
        private readonly MyConfig config;

        /// <summary>
        /// Constructor de la clase AESEncrytDecry
        /// </summary>
        /// <param name="configu">Inyecta modelo de configuración de la aplicación</param>
        public AESEncrytDecry(MyConfig configu)
        {
            this.config = configu;
        }

        /// <summary>
        /// Método que desencripta cadenas de bytes encriptadas con AES
        /// </summary>
        /// <param name="cipherText">Texto encriptado en formato bytes</param>
        /// <param name="key">llave de encriptación</param>
        /// <param name="iv">Vector de inicialización</param>
        /// <returns>String con el texto desencriptado</returns>
        public string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;


                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Método que encripta un texto en string y lo convierte en un arreglo de bytes
        /// </summary>
        /// <param name="plainText">texto en string</param>
        /// <param name="key">llave de encriptación</param>
        /// <param name="iv">vector de inicialización</param>
        /// <returns>Arreglo de bytes encriptados</returns>
        public byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.BlockSize = 128;
                rijAlg.KeySize = 128;


                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }

        /// <summary>
        /// Método para manejar la desencriptación de datos desde string base64 hacia string
        /// </summary>
        /// <param name="cipherText">texto encriptado en base64</param>
        /// <returns>textp desencriptado</returns>
        public string DecryptStringAES(string cipherText)
        {


            var keybytesHex = BigInteger.Parse(config.AES.key, System.Globalization.NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();

            var ivhex = BigInteger.Parse(config.AES.iv, System.Globalization.NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytesHex, ivhex);
            return string.Format(decriptedFromJavascript);
        }

        /// <summary>
        /// Método para manejo de encriptación desde un texto en string hacia el texto encriptado en formato base64
        /// </summary>
        /// <param name="cipherText">texto a encriptar</param>
        /// <returns>texto encriptado en formato base64</returns>
        public string EncryptStringAES(string cipherText)
            {
                var keybytesHex = BigInteger.Parse(config.AES.key, System.Globalization.NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();
                var ivhex = BigInteger.Parse(config.AES.iv, System.Globalization.NumberStyles.HexNumber).ToByteArray().Reverse().ToArray();
                var encrypted8 = EncryptStringToBytes(cipherText, keybytesHex, ivhex);
                String encriptado = Convert.ToBase64String(encrypted8);
                return encriptado;
            }
    }
}
