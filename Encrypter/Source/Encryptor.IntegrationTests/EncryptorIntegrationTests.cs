using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EncryptionHelpers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Xunit;

namespace Encryptor.IntegrationTests
{
    public class EncryptorIntegrationTests
    {
        [Fact]
        public void Encrypt_To_File_And_Decrypt()
        {
            var merchantSubscriptionKey = Guid.NewGuid().ToString();
            const string encryptedOutputFile = "merchant.enc";
            var merchantPublicKeyFileName = @"Certificates\merchant.crt";
            var mobilePayPublicKeyFileName = @"Certificates\mobilepay.crt";

            Encrypt.ToFile(merchantSubscriptionKey, merchantPublicKeyFileName, mobilePayPublicKeyFileName, encryptedOutputFile);

            //Decrypt file
            var csp = new RSACryptoServiceProvider();
            var streamReader = File.OpenText(@"Certificates\mobilepay.pvk");
            var pr = new PemReader(streamReader);

            var privateKey = (RsaPrivateCrtKeyParameters)pr.ReadObject();
            var rsa = DotNetUtilities.ToRSAParameters(privateKey);

            csp.ImportParameters(rsa);
            var encryptedFile = File.ReadAllBytes(encryptedOutputFile);

            var decrypted = Encoding.UTF8.GetString(RSAEncryptionService.Decrypt(csp, encryptedFile));

            var decryptedSplits = decrypted.Split(';');
            var decryptedMerchantPublicKey = decryptedSplits[0];
            var decryptedMerchantSubscriptionKey = decryptedSplits[1];

            var merchantPublicKey = File.ReadAllText(merchantPublicKeyFileName);

            Assert.Equal(merchantSubscriptionKey,decryptedMerchantSubscriptionKey);
            Assert.Equal(Regex.Replace(merchantPublicKey, @"\r\n?|\n", ""), Regex.Replace(decryptedMerchantPublicKey, @"\r\n?|\n", ""));
        }

        [Fact]
        public void Encrypt_To_File_Using_Main_Using_Default_Filename()
        {
            var merchantPublicKeyFileName = @"Certificates\merchant.crt";
            var mobilePayPublicKeyFileName = @"Certificates\mobilepay.crt";
            var merchantSubscriptionKey = Guid.NewGuid().ToString();

            string[] args = new [] { merchantSubscriptionKey, merchantPublicKeyFileName, mobilePayPublicKeyFileName };

            Program.Main(args);

            var csp = new RSACryptoServiceProvider();
            var streamReader = File.OpenText(@"Certificates\mobilepay.pvk");
            var pr = new PemReader(streamReader);

            var privateKey = (RsaPrivateCrtKeyParameters)pr.ReadObject();
            var rsa = DotNetUtilities.ToRSAParameters(privateKey);

            csp.ImportParameters(rsa);
            var encryptedFile = File.ReadAllBytes("merchant.enc");

            var decrypted = Encoding.UTF8.GetString(RSAEncryptionService.Decrypt(csp, encryptedFile));

            var decryptedSplits = decrypted.Split(';');
            var decryptedMerchantPublicKey = decryptedSplits[0];
            var decryptedMerchantSubscriptionKey = decryptedSplits[1];

            var merchantPublicKey = File.ReadAllText(merchantPublicKeyFileName);

            Assert.Equal(merchantSubscriptionKey, decryptedMerchantSubscriptionKey);
            Assert.Equal(Regex.Replace(merchantPublicKey, @"\r\n?|\n", ""), Regex.Replace(decryptedMerchantPublicKey, @"\r\n?|\n", ""));
        }

        [Fact]
        public void Encrypt_To_File_Using_Main_Using_Manually_Filename()
        {
            const string encryptedOutputFile = "manually.enc";
            var merchantPublicKeyFileName = @"Certificates\merchant.crt";
            var mobilePayPublicKeyFileName = @"Certificates\mobilepay.crt";
            var merchantSubscriptionKey = Guid.NewGuid().ToString();

            string[] args = new[] { merchantSubscriptionKey, merchantPublicKeyFileName, mobilePayPublicKeyFileName, encryptedOutputFile };

            Program.Main(args);

            var csp = new RSACryptoServiceProvider();
            var streamReader = File.OpenText(@"Certificates\mobilepay.pvk");
            var pr = new PemReader(streamReader);

            var privateKey = (RsaPrivateCrtKeyParameters)pr.ReadObject();
            var rsa = DotNetUtilities.ToRSAParameters(privateKey);

            csp.ImportParameters(rsa);
            var encryptedFile = File.ReadAllBytes(encryptedOutputFile);

            var decrypted = Encoding.UTF8.GetString(RSAEncryptionService.Decrypt(csp, encryptedFile));

            var decryptedSplits = decrypted.Split(';');
            var decryptedMerchantPublicKey = decryptedSplits[0];
            var decryptedMerchantSubscriptionKey = decryptedSplits[1];

            var merchantPublicKey = File.ReadAllText(merchantPublicKeyFileName);

            Assert.Equal(merchantSubscriptionKey, decryptedMerchantSubscriptionKey);
            Assert.Equal(Regex.Replace(merchantPublicKey, @"\r\n?|\n", ""), Regex.Replace(decryptedMerchantPublicKey, @"\r\n?|\n", ""));
        }
    }
}
