using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveEncryption
{
    private static byte[] _key;

    static SaveEncryption()
    {
        // キーのみ固定で生成（IVは毎回ランダム）
        string seed = "akumutogenjitsu_ringo_save_key_v1";
        using (SHA256 sha256 = SHA256.Create())
        {
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(seed));
        }
    }

    /// <summary>
    /// JSONをバイナリに変換して暗号化
    /// IVはランダム生成し、暗号化データの先頭に付加する
    /// </summary>
    public static byte[] EncryptJson(string json)
    {
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.GenerateIV(); // ランダムIVを生成
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            {
                // 最初にIVを書き込む（16バイト）
                ms.Write(aes.IV, 0, aes.IV.Length);

                // 暗号化データを書き込む
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(jsonBytes, 0, jsonBytes.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }
    }

    /// <summary>
    /// 暗号化されたバイナリを復号化してJSONに変換
    /// データの先頭16バイトからIVを抽出して使用
    /// </summary>
    public static string DecryptToJson(byte[] encryptedData)
    {
        // データサイズの検証
        if (encryptedData == null || encryptedData.Length < 16)
        {
            throw new ArgumentException("暗号化データが不正です（サイズが小さすぎます）");
        }

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // 先頭16バイトからIVを抽出
            byte[] iv = new byte[16];
            Array.Copy(encryptedData, 0, iv, 0, 16);
            aes.IV = iv;

            // 17バイト目以降が実際の暗号化データ
            int encryptedDataLength = encryptedData.Length - 16;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (MemoryStream ms = new MemoryStream(encryptedData, 16, encryptedDataLength))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (MemoryStream resultStream = new MemoryStream())
            {
                cs.CopyTo(resultStream);
                byte[] decryptedBytes = resultStream.ToArray();
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    // 以下、既存のEncrypt/Decryptメソッドは削除してOK
    // （EncryptJson/DecryptToJsonで完結するため）
}
