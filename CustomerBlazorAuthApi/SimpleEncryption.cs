using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SimpleEncryption
{
    // Use a raw byte array for the key (16 bytes for AES-128)
    private static readonly byte[] encryptionKey = new byte[] {
        0x48, 0x51, 0x77, 0x64, 0x6A, 0x59, 0x3C, 0x45,
        0x6E, 0x31, 0x67, 0x76, 0x61, 0x66, 0x58, 0x62
    }; // 16 bytes key for AES-128

    // Use a raw byte array for the IV (16 bytes)
    private static readonly byte[] iv = new byte[] {
        0x3B, 0x5E, 0xE4, 0x97, 0xD4, 0xC8, 0xC1, 0x62,
        0x78, 0xB9, 0x9A, 0x2C, 0x3F, 0x73, 0xA8, 0x4C
    }; // 16 bytes IV

    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(cs);
        writer.Write(plainText);
        writer.Close();
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }
}
