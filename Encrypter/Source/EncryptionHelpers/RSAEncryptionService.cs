using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace EncryptionHelpers
{
    public static class RSAEncryptionService
    {
        public static byte[] Encrypt(RSACryptoServiceProvider rsa, byte[] bytes)
        {
            var keySize = rsa.KeySize / 8;
            var maxLength = keySize - 42;
            var dataLength = bytes.Length;
            var iterations = dataLength / maxLength;

            var result = new List<byte>();
            for (int i = 0; i <= iterations; i++)
            {
                var tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                var encryptedBytes = rsa.Encrypt(tempBytes, true);
                
                result.AddRange(BitConverter.GetBytes(encryptedBytes.Length));
                result.AddRange(encryptedBytes);
            }

            return result.ToArray();
        }

        public static byte[] Decrypt(RSACryptoServiceProvider rsa, byte[] encBytes)
        {
            var list = new List<byte>();
            var i = 0;
            while (i < encBytes.Length)
            {
                var length = BitConverter.ToInt32(encBytes, i);
                i += sizeof(int);

                var encryptedBytes = new byte[length];
                Array.Copy(encBytes, i, encryptedBytes, 0, length);
                
                list.AddRange(rsa.Decrypt(encryptedBytes, true));

                i += length;
            }

            return list.ToArray();
        }
    }
}
