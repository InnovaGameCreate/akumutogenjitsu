using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// セーブデータの暗号化・復号化を行うユーティリティクラス
/// </summary>
public static class SaveEncryption
{
    // ゲーム起動時に動的生成するキー（より安全）
    private static byte[] _key;
    private static byte[] _iv;

    static SaveEncryption()
    {
        // 固定値からハッシュ生成（この値を変更してください）
        string seed = "akumutogenjitsu_ringo_save_key_v1";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));
            _key = hash; // 32バイト

            // IV用に別のシード
            string ivSeed = "akumutogenjitsu_ringo_iv_key_v1";
            byte[] ivHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(ivSeed));
            _iv = new byte[16];
            Array.Copy(ivHash, _iv, 16); // 最初の16バイトを使用
        }
    }

    /// <summary>
    /// データを暗号化する
    /// </summary>
    public static byte[] Encrypt(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
                return ms.ToArray();
            }
        }
    }

    /// <summary>
    /// データを復号化する
    /// </summary>
    public static byte[] Decrypt(byte[] encryptedData)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (MemoryStream ms = new MemoryStream(encryptedData))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (MemoryStream resultStream = new MemoryStream())
            {
                cs.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }

    /// <summary>
    /// JSONをバイナリに変換して暗号化
    /// </summary>
    public static byte[] EncryptJson(string json)
    {
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        return Encrypt(jsonBytes);
    }

    /// <summary>
    /// 暗号化されたバイナリを復号化してJSONに変換
    /// </summary>
    public static string DecryptToJson(byte[] encryptedData)
    {
        byte[] decryptedBytes = Decrypt(encryptedData);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
