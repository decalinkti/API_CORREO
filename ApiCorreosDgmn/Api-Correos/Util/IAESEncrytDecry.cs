
namespace Api_Correos.Util
{
    /// <summary>
    /// Interfaz para la clase AESEncrytDecry
    /// </summary>
    public interface IAESEncrytDecry
    {
        string DecryptStringAES(string cipherText);
        string EncryptStringAES(string cipherText);
    }
}
