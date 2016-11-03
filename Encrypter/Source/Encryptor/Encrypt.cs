using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using EncryptionHelpers;

namespace Encryptor
{
    public class Encrypt
    {
        public static void ToFile(
            string merchantSubscriptionKey, 
            string merchantPublicKeyFileName, 
            string mobilePayPublicKeyFileName, 
            string encryptedOutputFile = "merchant.enc")
        {
            if (string.IsNullOrWhiteSpace(merchantSubscriptionKey))
            {
                throw new ArgumentException("Argument 1, your merchant subscription key, is not valid");
            }

            string merchantPublicKeyCrt;
            try
            {
                var x509Certificate2 = new X509Certificate2(merchantPublicKeyFileName);
                merchantPublicKeyCrt = ExportToPem(x509Certificate2);
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "Argument 2, your merchant public key file path, is not pointing to a valid X509Certificate file", e);
            }

            X509Certificate2 mobilePayPublicKeyCrt;
            try
            {
                mobilePayPublicKeyCrt = new X509Certificate2(mobilePayPublicKeyFileName);
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "Argument 3, MobilePay's public key file path, is not pointing to a valid X509Certificate file", e);
            }

            var encryptedBytes = RSAEncryptionService.Encrypt(
                mobilePayPublicKeyCrt.PublicKey.Key as RSACryptoServiceProvider,
                Encoding.UTF8.GetBytes(merchantPublicKeyCrt + ';' + merchantSubscriptionKey));

            using (var outputFileStream = new FileStream(encryptedOutputFile, FileMode.Create, FileAccess.ReadWrite))
            {
                outputFileStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
        }

        private static string ExportToPem(X509Certificate cert)
        {
            var builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert),Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }
    }
}