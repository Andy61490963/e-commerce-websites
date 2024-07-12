using System.Security.Cryptography;

namespace Shopping.lib.Helpers;

public static class EncryptHelper
{
    /// <summary>
    /// AES_KEY_256
    /// </summary>
    private static readonly byte[] Key = {
                                             12, 163, 158, 118, 167, 57, 71, 6, 194, 160, 200, 234, 135, 167, 107, 239, 175, 33,
                                             87, 10, 158, 245, 121, 147, 120, 150, 73, 173, 187, 174, 127, 110
                                         };

    /// <summary>
    /// AES_KEY_IV_128
    /// </summary>
    private static readonly byte[] Iv = {
                                            92, 108, 167, 213, 109, 54, 192, 232, 111, 106, 108, 130, 69, 30, 2, 176
                                        };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string AesEncrypt( this string str )
    {
        using var aesAlgorithm = Aes.Create();

        var encryptor = aesAlgorithm.CreateEncryptor( Key, Iv );

        byte[] encryptedData;

        using ( var ms = new MemoryStream() )
        {
            using ( var cs = new CryptoStream( ms, encryptor, CryptoStreamMode.Write ) )
            {
                using ( var sw = new StreamWriter( cs ) )
                {
                    sw.Write( str );
                }

                encryptedData = ms.ToArray();
            }
        }

        return Convert.ToBase64String( encryptedData );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptedStr"></param>
    /// <returns></returns>
    public static string AesDecrypt(this string encryptedStr )
    {
        using var aesAlgorithm = Aes.Create();

        var decryptor = aesAlgorithm.CreateDecryptor( Key, Iv );

        var cipher = Convert.FromBase64String( encryptedStr );

        using var ms = new MemoryStream( cipher );
        using var cs = new CryptoStream( ms, decryptor, CryptoStreamMode.Read );
        using var sr = new StreamReader( cs );
        return sr.ReadToEnd();
    }

}