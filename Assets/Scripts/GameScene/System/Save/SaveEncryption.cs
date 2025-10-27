using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// セーブデータの暗号化・復号化を行うユーティリティクラス
/// AES-256 + HMAC-SHA256による改ざん検知付き
/// </summary>
public static class SaveEncryption
{
    private static byte[] _aesKey;
    private static byte[] _hmacKey;
    private static byte[] _salt;

    static SaveEncryption()
    {
        // ソルトの生成または読み込み
        if (!PlayerPrefs.HasKey("SaveEncryptionSalt"))
        {
            // 初回起動時：ランダムなソルトを生成
            _salt = GenerateRandomBytes(32);
            PlayerPrefs.SetString("SaveEncryptionSalt", Convert.ToBase64String(_salt));
            PlayerPrefs.Save();
        }
        else
        {
            // 2回目以降：保存されたソルトを読み込み
            _salt = Convert.FromBase64String(PlayerPrefs.GetString("SaveEncryptionSalt"));
        }

        string aesSeed = "akumutogenjitsu_ringo_save_key_v1";
        using (var pbkdf2 = new Rfc2898DeriveBytes(aesSeed, _salt, 100000, HashAlgorithmName.SHA256))
        {
            _aesKey = pbkdf2.GetBytes(32);
        }

        string hmacSeed = "akumutogenjitsu_ringo_hmac_key_v1";
        using (var pbkdf2 = new Rfc2898DeriveBytes(hmacSeed, _salt, 100000, HashAlgorithmName.SHA256))
        {
            _hmacKey = pbkdf2.GetBytes(32);
        }
    }

    /// <summary>
    /// 暗号学的に安全なランダムバイト列を生成
    /// </summary>
    private static byte[] GenerateRandomBytes(int length)
    {
        byte[] bytes = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }
        return bytes;
    }


    /// <summary>
    /// JSONをバイナリに変換して暗号化（HMAC付き）
    /// データ構造: [IV:16bytes] + [暗号化データ:可変] + [HMAC:32bytes]
    /// </summary>
    public static byte[] EncryptJson(string json)
    {
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (Aes aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] encryptedData;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(jsonBytes, 0, jsonBytes.Length);
                    cs.FlushFinalBlock();
                }
                encryptedData = ms.ToArray();
            }

            // IV + 暗号化データを結合
            byte[] dataWithIV = new byte[aes.IV.Length + encryptedData.Length];
            Buffer.BlockCopy(aes.IV, 0, dataWithIV, 0, aes.IV.Length);
            Buffer.BlockCopy(encryptedData, 0, dataWithIV, aes.IV.Length, encryptedData.Length);

            // ★HMAC-SHA256でタグを生成
            byte[] hmacTag = ComputeHMAC(dataWithIV);

            // 最終データ: [IV] + [暗号化データ] + [HMAC]
            byte[] finalData = new byte[dataWithIV.Length + hmacTag.Length];
            Buffer.BlockCopy(dataWithIV, 0, finalData, 0, dataWithIV.Length);
            Buffer.BlockCopy(hmacTag, 0, finalData, dataWithIV.Length, hmacTag.Length);

            return finalData;
        }
    }


    /// <summary>
    /// 暗号化されたバイナリを復号化してJSONに変換（HMAC検証付き）
    /// </summary>
    public static string DecryptToJson(byte[] encryptedData)
    {
        // データサイズの検証: IV(16) + 最小暗号文(16) + HMAC(32) = 64バイト
        if (encryptedData == null || encryptedData.Length < 64)
        {
            throw new ArgumentException("暗号化データが不正です（サイズが小さすぎます）");
        }

        // HMACタグを分離（末尾32バイト）
        int dataLength = encryptedData.Length - 32;
        byte[] dataWithIV = new byte[dataLength];
        byte[] receivedHMAC = new byte[32];

        Buffer.BlockCopy(encryptedData, 0, dataWithIV, 0, dataLength);
        Buffer.BlockCopy(encryptedData, dataLength, receivedHMAC, 0, 32);

        // HMACを検証（改ざん検知）
        byte[] computedHMAC = ComputeHMAC(dataWithIV);

        if (!FixedTimeEquals(receivedHMAC, computedHMAC))
        {
            throw new CryptographicException("データが改ざんされています！HMACの検証に失敗しました。");
        }

        // IVを抽出
        byte[] iv = new byte[16];
        Buffer.BlockCopy(dataWithIV, 0, iv, 0, 16);

        // 暗号化データを抽出
        int encryptedDataLength = dataWithIV.Length - 16;
        byte[] ciphertext = new byte[encryptedDataLength];
        Buffer.BlockCopy(dataWithIV, 16, ciphertext, 0, encryptedDataLength);

        // 復号化
        using (Aes aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (MemoryStream ms = new MemoryStream(ciphertext))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (MemoryStream resultStream = new MemoryStream())
            {
                cs.CopyTo(resultStream);
                byte[] decryptedBytes = resultStream.ToArray();
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

    /// <summary>
    /// HMAC-SHA256を計算
    /// </summary>
    private static byte[] ComputeHMAC(byte[] data)
    {
        using (HMACSHA256 hmac = new HMACSHA256(_hmacKey))
        {
            return hmac.ComputeHash(data);
        }
    }

    /// <summary>
    /// タイミング攻撃を防ぐための固定時間比較
    /// </summary>
    private static bool FixedTimeEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        int result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }
        return result == 0;
    }
}
